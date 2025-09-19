using InterviewBackEnd.Infrastructure;
using InterviewBackEnd.Model.Request;
using InterviewBackEnd.Model.Response;
using InterviewBackEnd.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InterviewBackEnd.Controllers
{
    [ApiController]
    [Route("api")]
    public class OrdersController : ControllerBase
    {

        private readonly ILogger<OrdersController> _logger;
        private readonly IOrderService _orderService;

        public OrdersController(ILogger<OrdersController> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        [HttpPost("orders", Name = "Orders")]
        [ServiceFilter(typeof(ValidateOrderRequestFilter))]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            // Add RequestId to logging scope for end-to-end tracking
            using (_logger.BeginScope(new Dictionary<string, object>()
            {
                { "RequestId", request.RequestId }
            }))
            {
                var response = await _orderService.CreateOrder(request);
                if (response.OrderId != Guid.Empty)
                {
                    // Return 201 Created with response body
                    return Created(string.Empty, response);
                }
                else
                {
                    return Ok(response);
                }
            }
        }
    }
}
