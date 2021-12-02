using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Service.Model.Moels
{
    public class ProductWithAllCategoryDTO
    {
        public ProductDTO ProductDTOItem { get; set; }
        
        public List<CategoryDTO> CategoryDTOList { get; set; }
    }
}
