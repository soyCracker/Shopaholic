using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.CMS.Model.Requests
{
    public class CategoryAddRequest
    {
        [SwaggerSchema("類別名稱")]
        public string Name { get; set; }
    }
}
