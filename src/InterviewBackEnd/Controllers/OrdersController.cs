using InterviewBackEnd.Infrastructure;
using InterviewBackEnd.Model.Request;
using InterviewBackEnd.Model.Response;
using Microsoft.AspNetCore.Mvc;

namespace InterviewBackEnd.Controllers
{
    [ApiController]
    [Route("api")]
    public class OrdersController : ControllerBase
    {

        private readonly ILogger<OrdersController> _logger;

        public OrdersController(ILogger<OrdersController> logger)
        {
            _logger = logger;
        }

        [HttpPost("orders", Name = "Orders")]
        [ServiceFilter(typeof(ValidateOrderRequestFilter))]
        public CreateOrderResponse CreateOrder([FromBody] CreateOrderRequest request)
        {
            // Add RequestId to logging scope for end-to-end tracking
            using (_logger.BeginScope(new Dictionary<string, object>()
            {
                { "RequestId",request.RequestId} 
            }
            ))
            {
                _logger.LogInformation("Start.");
                // Simulate order creation and return response with same RequestId
                return new CreateOrderResponse
                {
                    OrderId = request.OrderId,
                    ResponseId = request.RequestId, // Echo RequestId for end-to-end trace
                    ResponseMessage = "Order created successfully"
                };
            }
        }
    }
}
