using Swashbuckle.AspNetCore.Annotations;

namespace Shopaholic.CMS.Model.Requests
{
    public class ProductSearchRequest
    {
        [SwaggerSchema("商品名稱")]
        public string Name { get; set; }
        [SwaggerSchema("商品描述")]
        public string Description { get; set; }
        [SwaggerSchema("商品內容")]
        public string Content { get; set; }
    }
}
