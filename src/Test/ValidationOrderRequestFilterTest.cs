using InterviewBackEnd.Infrastructure;
using InterviewBackEnd.Model.POCOS;
using InterviewBackEnd.Model.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Test
{
    public class ValidateOrderRequestFilterTest
    {
        private ValidateOrderRequestFilter _filter;

        [SetUp]
        public void Setup()
        {
            _filter = new ValidateOrderRequestFilter();
        }

        private ActionExecutingContext CreateContext(CreateOrderRequest request)
        {
            var actionArguments = new Dictionary<string, object> { { "request", request } };
            var actionContext = new ActionContext
            {
                HttpContext = new DefaultHttpContext(),
                RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
                ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor()
            };
            return new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                actionArguments,
                new object());
        }

        [Test]
        public void OnActionExecuting_ValidRequest_DoesNotSetResult()
        {
            var request = new CreateOrderRequest
            {
                OrderId = Guid.NewGuid(),
                CustomerName = "Test",
                Items = new List<Items> { new Items { ProductId = 1, Quantity = 2 } }
            };
            var context = CreateContext(request);

            _filter.OnActionExecuting(context);

            Assert.IsNull(context.Result);
        }

        [Test]
        public void OnActionExecuting_MissingOrderId_SetsBadRequest()
        {
            var request = new CreateOrderRequest
            {
                OrderId = Guid.Empty,
                CustomerName = "Test",
                Items = new List<Items> { new Items { ProductId = 1, Quantity = 2 } }
            };
            var context = CreateContext(request);

            _filter.OnActionExecuting(context);

            Assert.IsInstanceOf<BadRequestObjectResult>(context.Result);
        }

        [Test]
        public void OnActionExecuting_MissingCustomerName_SetsBadRequest()
        {
            var request = new CreateOrderRequest
            {
                OrderId = Guid.NewGuid(),
                CustomerName = "",
                Items = new List<Items> { new Items { ProductId = 1, Quantity = 2 } }
            };
            var context = CreateContext(request);

            _filter.OnActionExecuting(context);

            Assert.IsInstanceOf<BadRequestObjectResult>(context.Result);
        }

        [Test]
        public void OnActionExecuting_NoItems_SetsBadRequest()
        {
            var request = new CreateOrderRequest
            {
                OrderId = Guid.NewGuid(),
                CustomerName = "Test",
                Items = new List<Items>()
            };
            var context = CreateContext(request);

            _filter.OnActionExecuting(context);

            Assert.IsInstanceOf<BadRequestObjectResult>(context.Result);
        }

        [Test]
        public void OnActionExecuting_InvalidItemQuantity_SetsBadRequest()
        {
            var request = new CreateOrderRequest
            {
                OrderId = Guid.NewGuid(),
                CustomerName = "Test",
                Items = new List<Items> { new Items { ProductId = 1, Quantity = 0 } }
            };
            var context = CreateContext(request);

            _filter.OnActionExecuting(context);

            Assert.IsInstanceOf<BadRequestObjectResult>(context.Result);
        }
    }
}