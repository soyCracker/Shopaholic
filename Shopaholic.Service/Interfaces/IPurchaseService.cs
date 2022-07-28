using Shopaholic.Web.Model.Requests;

namespace Shopaholic.Service.Interfaces
{
    public interface IPurchaseService
    {
        PurchaseType PType { get; }
        Task<string> CreateOrder(PurchaseOrderCreateReq req);
        Task<T> Pay<T>(PurchasePayReq req) where T : class;
        bool PayConfirm<T>(T req);
    }

    public enum PurchaseType
    {
        LinePay,
        EcPay
    }
}
