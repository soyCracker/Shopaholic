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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shopaholic.CMS.Common.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<LoggerFactory> logger)
        {
            this.logger = logger;
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
            bool isApi = Regex.IsMatch(context.Request.Path.Value, "/api/", RegexOptions.IgnoreCase);

            if(isApi)
            {
                await ReturnApiException(context, exception);
            }
            else
            {
                context.Response.Redirect("/Home/Error");
            }
            
        }

        private async Task ReturnApiException(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            string message = "Internal Server Error from the custom middleware.";

            if (exception is DbUpdateException)
            {
                message = ((DbUpdateException)exception).Message;
                logger.LogError("HandleExceptionAsync() " + message);
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
