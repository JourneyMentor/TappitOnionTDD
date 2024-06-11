using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GlobalResultPattern.SuccessResult
{
    public class SuccessMiddleware
    {
        private readonly RequestDelegate _next;

        public SuccessMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            await _next(context);

            context.Response.Body = originalBodyStream;
            memoryStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();

            if (context.Response.StatusCode >= 200 && context.Response.StatusCode < 300)
            {
                string modifiedResponseBody = "";
                object dataContent = responseBody;

                try
                {
                    dataContent = JsonSerializer.Deserialize<JsonElement>(responseBody);
                }
                catch
                {
                    if (string.IsNullOrWhiteSpace(responseBody))
                    {
                        dataContent = new object(); 
                    }
                }

                var responseWrapper = new Dictionary<string, object>
            {
                { "Success", true },
                { "Data", dataContent }
            };

                modifiedResponseBody = JsonSerializer.Serialize(responseWrapper);
                context.Response.ContentType = "application/json"; 
                await context.Response.WriteAsync(modifiedResponseBody, Encoding.UTF8);
            }
            else
            {
                await context.Response.WriteAsync(responseBody, Encoding.UTF8);
            }
        }
    }
}