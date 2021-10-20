using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.CMS.Model.Requests
{
    public class ProductGetRequest
    {
        [SwaggerSchema("商品ID")]
        public int Id { get; set; }
    }
}
