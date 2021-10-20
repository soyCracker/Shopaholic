using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.CMS.Model.Requests
{
    public class CategoryUpdateRequest
    {
        [SwaggerSchema("類別名稱")]
        public int Id { get; set; }
        [SwaggerSchema("類別ID")]
        public string Name { get; set; }
    }
}
