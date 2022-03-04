using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Web.Model.Requests
{
    public class OrderApplyReturnReq
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string OrderId { get; set; }
    }
}
