using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Web.Model.Requests
{
    public class ProductSearchByCategoryReq
    {
        [SwaggerSchema("類別Id")]
        public string CategoryId { get; set; }
        [SwaggerSchema("搜索字串")]
        public string SearchStr { get; set; }
        [SwaggerSchema("頁數")]
        public int Page { get; set; }
        [SwaggerSchema("每頁數量")]
        public int PageSize { get; set; }
    }
}
