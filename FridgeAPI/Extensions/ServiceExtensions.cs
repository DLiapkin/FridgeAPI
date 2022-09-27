using Contacts;
using LoggerService;
using Microsoft.Extensions.DependencyInjection;

namespace FridgeAPI.Extensions
{
    public static class ServiceExtensions
    {    
        public static void ConfigureLoggerService(this IServiceCollection services) 
        {
            services.AddScoped<ILoggerManager, LoggerManager>(); 
        }
    }   
}
