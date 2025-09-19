namespace InterviewBackEnd.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection RegisterFilters(this IServiceCollection services)
        {
            services.AddScoped<ValidateOrderRequestFilter>();
            return services;
        }
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestResponseLoggingMiddleware>();
        }

        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
