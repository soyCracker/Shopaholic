using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Web;

namespace Shopaholic.Service.Common.Filters
{
    public class SwaggerIgnoreFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var ignoreApis = context.ApiDescriptions.Where(wh => wh.CustomAttributes().Any(any => any is SwaggerIgnoreAttribute));
            if (ignoreApis != null)
            {
                foreach (var ignoreApi in ignoreApis)
                {
                    swaggerDoc.Paths.Remove("/" + ignoreApi.RelativePath);
                }
            }
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class SwaggerIgnoreAttribute : Attribute
    {

    }
}
