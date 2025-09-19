using AutoMapper;
using InterviewBackEnd.AutoMapperProfile;
using InterviewBackEnd.DataAccess;
using InterviewBackEnd.Service.Implementation;
using InterviewBackEnd.Service.Interface;
using Microsoft.EntityFrameworkCore;

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

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddAutoMapper(x =>
            {
                x.AddProfile<OrderProfile>();
            });
            services.AddScoped<IOrderService, OrderService>();
            return services;
        }

        public static IServiceCollection RegisterDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddPooledDbContextFactory<OrderProcessContext>(x =>
            {
                x.UseSqlServer(connectionString);
            });
            return services;
        }
    }
}
