using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Web.Model.Requests
{
    public class PurchaseReq
    {
        public List<PurchaseProductModel> ProductList { get; set; }
        [Required]
        public int OrderTypeCode { get; set; }
        public string UserId { get; set; }
        [Required]
        public string ReceiveMan { get; set; }
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
