using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicyManagement.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddSWaggerExtension(this IServiceCollection services)
        {           
            services.AddSwaggerGen(s =>
            {
                s.IncludeXmlComments(string.Format(@"{0}\PolicyManagement.xml", System.AppDomain.CurrentDomain.BaseDirectory));

                s.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "V1",
                    Title = "PolicyManagement",
                    Description = "This Api will be responsible for overall data distribution for the policy management system."
                });
            });         
        }

        public static void AddApiVersioningExtension(this IServiceCollection servcies)
        {
            servcies.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });
        }
    }
}
