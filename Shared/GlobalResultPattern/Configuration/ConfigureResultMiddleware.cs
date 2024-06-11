using GlobalResultPattern.ExceptionResult;
using GlobalResultPattern.SuccessResult;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace GlobalResultPattern.Configuration
{
    public static class ConfigureResultMiddleware
    {
        public static void ConfigureResultHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<SuccessMiddleware>();
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
