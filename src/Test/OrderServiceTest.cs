using AutoMapper;
using Azure;
using InterviewBackEnd.AutoMapperProfile;
using InterviewBackEnd.DataAccess;
using InterviewBackEnd.Model.DAO;
using InterviewBackEnd.Model.POCOS;
using InterviewBackEnd.Model.Request;
using InterviewBackEnd.Model.Response;
using InterviewBackEnd.Service.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    public class OrderServiceTest
    {
        private Mock<ILogger<OrderService>> _loggerMock;
        private Mock<ILoggerFactory> _loggerFactoryMock;
        private Mock<IDbContextFactory<OrderProcessContext>> _mockDbContextFactory;
        private IMapper _mapper;
        private OrderProcessContext _context;
        private OrderProcessContext _secondContext;
        private OrderService _orderService;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<OrderService>>();
            _loggerFactoryMock = new Mock<ILoggerFactory>();
            _mockDbContextFactory = new Mock<IDbContextFactory<OrderProcessContext>>();
            _loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>()))
                .Returns(_loggerMock.Object);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new OrderProfile());
            }, _loggerFactoryMock.Object);
            _mapper = config.CreateMapper();

            var options = new DbContextOptionsBuilder<OrderProcessContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new OrderProcessContext(options);
            _secondContext = new OrderProcessContext(options);
            _mockDbContextFactory.Setup(x => x.CreateDbContextAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(_context);
            // Seed ProductStock
            _context.ProductStock.Add(new Stock { Id = 1, ProductName = "ProductA", Inventory = 10 });
            _context.ProductStock.Add(new Stock { Id = 2, ProductName = "ProductB", Inventory = 5 });
            _context.SaveChanges();

            _orderService = new OrderService(_loggerMock.Object, _mapper, _mockDbContextFactory.Object);
        }

        [Test]
        public async Task CreateOrder_SuccessfulOrder_ReturnsResponseWithOrderId()
        {
            var request = new CreateOrderRequest
            {
                OrderId = Guid.NewGuid(),
                CustomerName = "Test Customer",
                CreatedAt = DateTime.UtcNow,
                Items = new List<Items>
                {
                    new Items { ProductId = 1, Quantity = 2 },
                    new Items { ProductId = 2, Quantity = 1 }
                }
            };

            var response = await _orderService.CreateOrder(request);

            Assert.AreEqual(request.OrderId, response.ResponseId);
            Assert.AreEqual("Success", response.ResponseMessage);
            Assert.IsInstanceOf<CreateOrderResponse>(response);
        }

        [Test]
        public async Task CreateOrder_ProductDoesNotExist_ReturnsResponseWithRequestId()
        {
            var request = new CreateOrderRequest
            {
                OrderId = Guid.NewGuid(),
                CustomerName = "Test Customer",
                CreatedAt = DateTime.UtcNow,
                Items = new List<Items>
                {
                    new Items { ProductId = 99, Quantity = 1 } // Non-existent product
                }
            };

            var response = await _orderService.CreateOrder(request);

            Assert.AreEqual(request.RequestId, response.ResponseId);
            Assert.AreEqual(response.OrderId, Guid.Empty);
        }

        [Test]
        public async Task CreateOrder_InsufficientInventory_ReturnsEmptyResponse()
        {
            var request = new CreateOrderRequest
            {
                OrderId = Guid.NewGuid(),
                CustomerName = "Test Customer",
                CreatedAt = DateTime.UtcNow,
                Items = new List<Items>
                {
                    new Items { ProductId = 1, Quantity = 100 } // More than available
                }
            };

            var response = await _orderService.CreateOrder(request);

            Assert.IsNotNull(response);
            Assert.AreEqual(response.OrderId, Guid.Empty);
            Assert.True(response.ResponseMessage.Contains("InsufficientInventory"));
        }
        [Test]
        public async Task CreateOrder_Idempotence_SameOrderTwice_DoesNotCreateDuplicate()
        {
            var orderId = Guid.NewGuid();
            var request = new CreateOrderRequest
            {
                OrderId = orderId,
                CustomerName = "Idempotent Customer",
                CreatedAt = DateTime.UtcNow,
                Items = new List<Items>
                {
                    new Items { ProductId = 1, Quantity = 2 }
                }
            };

            // First call
            var response1 = await _orderService.CreateOrder(request);

            var orderServiceCreatedLater = new OrderService(_loggerMock.Object, _mapper, _mockDbContextFactory.Object);
            _mockDbContextFactory.Setup(x => x.CreateDbContextAsync(It.IsAny<CancellationToken>()))
               .ReturnsAsync(_secondContext);
            // Second call with the same OrderId and data
            var response2 = await orderServiceCreatedLater.CreateOrder(request);

            // Both responses should have the same OrderId
            Assert.AreEqual(response1.OrderId, response2.OrderId);
            Assert.AreEqual("Success", response1.ResponseMessage);
            Assert.AreEqual("Success", response2.ResponseMessage);

        }
    }
}