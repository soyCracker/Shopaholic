using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shopaholic.Web.Model.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shopaholic.Web.Common.Middlewares
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

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);

                // 404 NotFound
                if (httpContext.Response.StatusCode == (int)HttpStatusCode.NotFound)
                {
                    httpContext.Response.Redirect("/Error/Error404");
                }
                else if(httpContext.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    httpContext.Response.Redirect("/Error/Error401");
                }
            }
            catch (Exception ex)
            {
                await HandleException(httpContext, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception exception)
        {
            bool isApi = Regex.IsMatch(context.Request.Path.Value, "/api/", RegexOptions.IgnoreCase);

            // API Exception
            if (isApi)
            {
                await ReturnApiException(context, exception);
            }
            // Any other Exception
            else
            {
                context.Response.Redirect("/Error/Error500");
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
