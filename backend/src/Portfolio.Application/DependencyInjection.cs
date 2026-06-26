using Microsoft.Extensions.DependencyInjection;
using Portfolio.Application.Common.Interfaces;
using Portfolio.Application.Services;

namespace Portfolio.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IPortfolioService, PortfolioService>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IGalleryService, GalleryService>();
            services.AddScoped<IDashboardService, DashboardService>();

            return services;
        }
    }
}
