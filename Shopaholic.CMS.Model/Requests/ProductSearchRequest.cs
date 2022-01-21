using Swashbuckle.AspNetCore.Annotations;

namespace Shopaholic.CMS.Model.Requests
{
    public class ProductSearchRequest
    {
        [SwaggerSchema("商品名稱")]
        public string Name { get; set; } = null;
        [SwaggerSchema("商品描述")]
        public string Description { get; set; } = null;
        [SwaggerSchema("商品內容")]
        public string Content { get; set; } = null;
        [SwaggerSchema("頁數")]
        public int Page { get; set; } 
        [SwaggerSchema("每頁數量")]
        public int PageSize { get; set; } = 20;
    }
}
