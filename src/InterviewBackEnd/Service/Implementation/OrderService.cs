using AutoMapper;
using InterviewBackEnd.DataAccess;
using InterviewBackEnd.Model.DAO;
using InterviewBackEnd.Model.Request;
using InterviewBackEnd.Model.Response;
using InterviewBackEnd.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace InterviewBackEnd.Service.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly ILogger<OrderService> _logger;
        private readonly IMapper _mapper;
        private readonly OrderProcessContext _orderProcessContext;
        public OrderService(ILogger<OrderService> logger, IMapper mapper, OrderProcessContext orderProcessContext)
        {
            _logger = logger;
            _mapper = mapper;
            _orderProcessContext = orderProcessContext; ;
        }
        public async Task<CreateOrderResponse> CreateOrder(CreateOrderRequest request)
        {
            var response = new CreateOrderResponse()
            {
                OrderId = Guid.Empty,
            };
            response.ResponseId = request.RequestId;
            var orderToBeCreated = _mapper.Map<Order>(request);
            await _orderProcessContext.Orders.AddAsync(orderToBeCreated);
            // Verify if all the product id exist 
            var existProduct = await _orderProcessContext.ProductStock
                .Where(x => request.Items.Select(z => z.ProductId)
                .Contains(x.Id)).ToListAsync();
            var exsitProductId = existProduct.Select(x => x.Id);
            var productIdToBePurchased = request.Items.Select(x => x.ProductId);
            if (exsitProductId.OrderBy(x => x).SequenceEqual(productIdToBePurchased.OrderBy(x => x)))
            {
                foreach (var itemToBePuchased in orderToBeCreated.OrderedItems)
                {
                    var productStock = existProduct.First(x => x.Id == itemToBePuchased.Id);
                    if (productStock.Inventory < itemToBePuchased.Quantity)
                    {
                        return new CreateOrderResponse()
                        {
                            ResponseMessage = $"InsufficientInventory ProductId {itemToBePuchased.Id}."
                        };
                    }
                    else
                    {
                        productStock.Inventory -= itemToBePuchased.Quantity;
                    }
                }
                await _orderProcessContext.SaveChangesAsync();
                response.ResponseId = request.OrderId;
                response.OrderId = request.OrderId;
                response.ResponseMessage = "Success";
                return response;
            }
            else
            {
                var productIdNotexist = request.Items.Select(x => x.ProductId)
                    .Except(existProduct.Select(z => z.Id)).ToList();
                using (_logger.BeginScope(new Dictionary<string, Object>()
                { 
                    {"ProductIdNotExist",productIdNotexist } 
                }))
                {
                    _logger.LogWarning("Some of the ProdcutId Not Exist.");
                };
                response.ResponseMessage = $"ProductDoesNotExist {String.Join(",", productIdNotexist)}";
                return response;
            }
        }
    }
}
