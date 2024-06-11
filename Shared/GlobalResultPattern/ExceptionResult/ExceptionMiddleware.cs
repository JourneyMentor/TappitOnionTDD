using FluentValidation;
using Microsoft.AspNetCore.Http;
using SendGrid.Helpers.Errors.Model;
using System.Text.Json;

namespace GlobalResultPattern.ExceptionResult
{
    public class ExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            int statusCode = GetStatusCode(exception);
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;

            var responseContent = new ExceptionModel
            {
                Success = false,  // Hata durumunda başarı durumunu belirtmek
                StatusCode = statusCode
            };

            if (exception is ValidationException validationException)
            {
                responseContent.Errors = validationException.Errors.Select(e => e.ErrorMessage).ToList();
                responseContent.StatusCode = StatusCodes.Status422UnprocessableEntity;
            }
            else
            {
                responseContent.Errors = new List<string> { $"Error Message: {exception.Message}" };
            }

            var responseJson = JsonSerializer.Serialize(responseContent);

            // Log response
          //  Console.WriteLine("Sending response: " + responseJson);

            await httpContext.Response.WriteAsync(responseJson);
        }

        private static int GetStatusCode(Exception exception) =>
            exception switch
            {
                BadRequestException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status400BadRequest,
                ValidationException => StatusCodes.Status422UnprocessableEntity,
                _ => StatusCodes.Status500InternalServerError
            };
    }

}
