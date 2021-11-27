using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.CMS.Model.Requests
{
    public class ProductAddRequest
    {
        [SwaggerSchema("商品名稱")]
        [Required]
        public string Name { get; set; }
        [SwaggerSchema("商品描述")]
        public string Description { get; set; }
        [SwaggerSchema("商品內容")]
        public string Content { get; set; }
        [SwaggerSchema("商品價格")]
        [Required]
        public int Price { get; set; }
        [SwaggerSchema("商品庫存")]
        [Required]
        public int Stock { get; set; }
        [SwaggerSchema("商品圖片路徑")]
        public string Image { get; set; }
        [SwaggerSchema("商品類別ID")]
        public int CategoryId { get; set; }
    }
}
