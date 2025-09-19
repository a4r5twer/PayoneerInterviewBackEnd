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
        public CreateOrderResponse CreateOrder([FromBody] CreateOrderRequest request)
        {
            return new CreateOrderResponse();
        }
    }
}
