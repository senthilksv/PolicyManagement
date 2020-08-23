using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicyManagement.Api.Extensions
{
    public static class AppExtensions
    {
        public static void AddSwaggerExtension(this IApplicationBuilder app)
        {
            //Enable middleware to serve generated swagger as JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "PolicyManagementSystem Api");
            });
        }
    }
}
