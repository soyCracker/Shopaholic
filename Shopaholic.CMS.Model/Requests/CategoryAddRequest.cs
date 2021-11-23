using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Shopaholic.CMS.Model.Requests
{
    public class CategoryAddRequest
    {
        [SwaggerSchema("類別名稱")]
        [Required]
        public string Name { get; set; }
    }
}
