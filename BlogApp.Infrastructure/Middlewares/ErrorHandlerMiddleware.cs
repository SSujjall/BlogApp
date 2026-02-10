using BlogApp.Application.Exceptions;
using BlogApp.Application.Helpers.HelperModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace BlogApp.Infrastructure.Middlewares
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

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Set the response status code
            context.Response.ContentType = "application/json";

            // Service excetpions
            if (exception is ServiceException serviceEx)
            {
                context.Response.StatusCode = (int)serviceEx.StatusCode;

                var businessExMessage = "Request failed.";
                var businessStatusCode = context.Response.StatusCode;

                var businessRes = ApiResponse<string>.Failed(
                    serviceEx.Errors,
                    businessExMessage,
                    (HttpStatusCode)businessStatusCode
                );

                var json = JsonSerializer.Serialize(
                    businessRes, 
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                return context.Response.WriteAsync(json);
            }

            // Unknown exception
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var errors = new Dictionary<string, string>
            {
                { "SystemError", exception.Message }
            };
            var message = "An unexpected error occurred. Please try again later.";
            var statusCode = context.Response.StatusCode;

            var errorRes = ApiResponse<string>.Failed(
                errors,
                message,
                (HttpStatusCode)statusCode
            );

            var jsonResponse = JsonSerializer.Serialize(
                errorRes, 
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            return context.Response.WriteAsync(jsonResponse);
        }
    }

    public static class ErrorHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
