﻿using Contacts;
using FridgeAPI.Repositories;
using LoggerService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
                opts.UseSqlServer(configuration.GetConnectionString("sqlConnection"));
                opts.UseLazyLoadingProxies();
            });
        }
    }   
}