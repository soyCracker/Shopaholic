﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Web.Model.Requests
{
    public class PurchaseProductListModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int CurrentPrice { get; set; }
    }
}
