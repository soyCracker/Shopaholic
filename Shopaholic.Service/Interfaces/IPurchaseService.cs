using Shopaholic.Web.Model.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Service.Interfaces
{
    public interface IPurchaseService
    {
        string CreateOrder(PurchaseReq req);
        bool Pay(int price);
    }
}
