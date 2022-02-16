using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Service.Model.Moels
{
    public class OrderSearchResDTO
    {
        public List<OrderHeaderDTO> OrderHeaderDTOs { get; set; }

        public int TotalPages { get; set; }
    }
}
