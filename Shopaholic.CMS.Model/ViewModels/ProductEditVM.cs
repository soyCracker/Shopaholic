using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.CMS.Model.ViewModels
{
    public class ProductEditVM
    {
        public List<CategoryVM> CategoryListVM { get; set; }
        public ProductVM ProductVM { get; set;}
    }
}
