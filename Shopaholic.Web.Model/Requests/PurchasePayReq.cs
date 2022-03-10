using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Web.Model.Requests
{
    public class PurchasePayReq
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public int Price { get; set; }
        [Required]
        public string OrderId { get; set; }
        public string ConfirmUrl { get; set; }
    }
}
