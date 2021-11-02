using Swashbuckle.AspNetCore.Annotations;

namespace Shopaholic.CMS.Model.ViewModels
{
    public class CategoryVM
    {
        [SwaggerSchema("類別ID")]
        public int Id { get; set; }
        [SwaggerSchema("類別名稱")]
        public string Name { get; set; }
    }
}
