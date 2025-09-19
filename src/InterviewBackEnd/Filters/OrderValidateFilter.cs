using InterviewBackEnd.Model.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace InterviewBackEnd.Infrastructure
{
    public class ValidateOrderRequestFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue("request", out var value) && value is CreateOrderRequest request)
            {
                var errors = new List<string>();

                if (request.OrderId == Guid.Empty)
                    errors.Add("OrderId is required.");

                if (string.IsNullOrWhiteSpace(request.CustomerName))
                    errors.Add("CustomerName is required.");

                if (request.Items == null || !request.Items.Any())
                    errors.Add("At least one item is required.");

                if (request.Items != null)
                {
                    foreach (var item in request.Items)
                    {
                        if (item.ProductId <= 0)
                            errors.Add("ProductId must be greater than zero.");
                        if (item.Quantity <= 0)
                            errors.Add("Quantity must be greater than zero.");
                    }
                }

                if (errors.Any())
                {
                    context.Result = new BadRequestObjectResult(new
                    {
                        Errors = errors
                    });
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No post-processing required
        }
    }
}