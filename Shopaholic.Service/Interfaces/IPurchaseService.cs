using Shopaholic.Web.Model.Requests;
using Shopaholic.Web.Model.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Service.Interfaces
{
    public interface IPurchaseService
    {
        Task<string> CreateOrder(PurchaseOrderCreateReq req);
        Task<T> Pay<T>(PurchasePayReq req) where T : class;
        bool PayConfirm<T>(T req);
    }
}
