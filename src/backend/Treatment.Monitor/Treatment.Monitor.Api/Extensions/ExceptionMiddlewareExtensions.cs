using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Treatment.Monitor.Models;

namespace Treatment.Monitor.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFailure = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFailure == null)
                    {
                        return;
                    }

                    await context.Response.WriteAsync(new ErrorResult(contextFailure.Error.Message).ToString());
                });
            });
        }
    }
}