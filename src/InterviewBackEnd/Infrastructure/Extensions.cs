namespace InterviewBackEnd.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection RegisterFilters(this IServiceCollection services)
        {
            services.AddScoped<ValidateOrderRequestFilter>();
            return services;
        }

    }
}
