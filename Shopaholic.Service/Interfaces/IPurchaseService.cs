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
        Task<PurchasePayRes> Pay(PurchasePayReq req);
        bool PayConfirm(PurchaseConfirmReq req);
    }
}
