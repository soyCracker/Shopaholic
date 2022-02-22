using Shopaholic.Service.Interfaces;
using Shopaholic.Web.Model.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Service.Services
{
    public class CreditCardPurchaseService : IPurchaseService
    {
        public string CreateOrder(PurchaseReq req)
        {
            throw new NotImplementedException();
        }

        public bool Pay(int price)
        {
            throw new NotImplementedException();
        }
    }
}
