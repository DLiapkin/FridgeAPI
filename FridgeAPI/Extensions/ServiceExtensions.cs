using LoggerService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.Contracts;
using Domain.Contracts;
using Domain.Repositories;

namespace FridgeAPI.Extensions
{
    public static class ServiceExtensions
    {    
        public static void ConfigureLoggerService(this IServiceCollection services) 
        {
            services.AddScoped<ILoggerManager, LoggerManager>(); 
        }

        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(opts => 
            {
                opts.UseSqlServer(configuration.GetConnectionString("sqlConnection"), 
                    b => b.MigrationsAssembly("FridgeAPI"));
                opts.UseLazyLoadingProxies();
            });
        }

        public static void ConfigureRepositoryManager(this IServiceCollection services)
        { 
            services.AddScoped<IFridgeRepository, FridgeRepository>();
            services.AddScoped<IFridgeModelRepository, FridgeModelRepository>();
            services.AddScoped<IFridgeProductRepository, FridgeProductRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void ConfigureControllerServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IFridgeModelService, FridgeModelService>();
            services.AddScoped<IFridgeService, FridgeService>();
        }
    }   
}
