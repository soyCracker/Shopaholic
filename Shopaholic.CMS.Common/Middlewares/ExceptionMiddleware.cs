using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shopaholic.CMS.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shopaholic.CMS.Common.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {

            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
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

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            string message = "Internal Server Error from the custom middleware.";

            if (exception is DbUpdateException)
            {
                message = ((DbUpdateException)exception).Message;
            }
            

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(new MessageModel<string>()
                {
                    StatusCode = context.Response.StatusCode,
                    Success = false,
                    Msg = message
                }));
        }
    }
}
