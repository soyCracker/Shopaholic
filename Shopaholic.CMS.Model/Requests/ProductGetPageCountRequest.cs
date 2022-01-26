using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.CMS.Model.Requests
{
    public class ProductGetPageCountRequest
    {
        [SwaggerSchema("每頁長度")]
        public int PageSize { get; set; }
    }
}
