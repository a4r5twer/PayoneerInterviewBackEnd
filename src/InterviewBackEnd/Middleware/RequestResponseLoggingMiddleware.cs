using System.Text;

namespace InterviewBackEnd.Infrastructure
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log Request
            context.Request.EnableBuffering();
            var requestBody = await new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true).ReadToEndAsync();
            context.Request.Body.Position = 0;
            using (_logger.BeginScope(new Dictionary<string, object>()
            {
                { "RequestMessage",requestBody }
            }))
            {
                _logger.LogInformation("HTTP Request Information");
            }

            // Log Response
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            using (_logger.BeginScope(new Dictionary<string, object>()
            {
                { "ResponseMessage",responseText},
                {"StatusCode",context.Response.StatusCode }
            }))
            {
                _logger.LogInformation("HTTP Response Information.");
            }
            
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}