using Swashbuckle.AspNetCore.Annotations;

namespace Shopaholic.CMS.Model.Requests
{
    public class ProductSearchRequest
    {
        [SwaggerSchema("搜索字串")]
        public string SearchStr { get; set; }
        [SwaggerSchema("頁數")]
        public int Page { get; set; }
        [SwaggerSchema("每頁數量")]
        public int PageSize { get; set; }
    }
}
