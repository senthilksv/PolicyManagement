using Application.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PolicyManagement.Api.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (CosmosException error) when (error.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogError(error.Message, error);
                var response = context.Response;
                response.ContentType = "application/json";

                var responseModel = new ApiResponse<string>() { Succeeded = false, Message = "Item not found for the given id" };

                response.StatusCode = (int)HttpStatusCode.BadRequest;

                var result = JsonConvert.SerializeObject(responseModel);

                await response.WriteAsync(result);
            }
            catch (Exception error)
            {
                _logger.LogError(error.Message, error);
                var response = context.Response;
                response.ContentType = "application/json";

                var responseModel = new ApiResponse<string>() { Succeeded = false, Message = error?.Message };

                response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var result = JsonConvert.SerializeObject(responseModel);

                await response.WriteAsync(result);
            }
        }
    }
}
