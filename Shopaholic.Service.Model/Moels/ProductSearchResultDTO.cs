using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Service.Model.Moels
{
    public class ProductSearchResultDTO
    {
        public List<ProductDTO> ProductDTOs { get; set; }

        public int TotalPages { get; set; }
    }
}
