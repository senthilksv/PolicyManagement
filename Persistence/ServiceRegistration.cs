using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AzureCosmosDbOptions>(
               options =>
               {
                   options.DatabaseId = configuration["Azure:CosmosDb:DatabaseId"]; // Could read from key vault if you wanted to  
                   options.Key = configuration["Azure:CosmosDb:Key"];
                   options.Endpoint = configuration["Azure:CosmosDb:Endpoint"];
               });

            services.AddTransient<IPolicyRepository, PolicyRepository>();
        }
    }
}
