using Shopaholic.Entity.Models;
using Shopaholic.Service.Common.Business;
using Shopaholic.Service.Common.Constants;
using Shopaholic.Service.Interfaces;
using Shopaholic.Web.Model.Requests;
using Shopaholic.Web.Model.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Service.Services
{
    public class TestPurchaseService : IPurchaseService
    {
        private readonly ShopaholicContext dbContext;

        public TestPurchaseService(ShopaholicContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<string> CreateOrder(PurchaseOrderCreateReq req)
        {
            using (dbContext)
            {
                //// TODO 未來要改成獨立Server建立訂單編號
                string orderId = OrderBusiness.CreateOrder(dbContext, req);
                dbContext.SaveChanges();
                return orderId;
            }
        }

        public Task<PurchasePayRes> Pay(PurchasePayReq req)
        {
            throw new NotImplementedException();
        }

        public bool PayConfirm(PurchaseConfirmReq req)
        {
            using (dbContext)
            {
                var orderHeader = dbContext.OrderHeaders.SingleOrDefault(x => x.OrderId==req.SelfOrderId);
                if(orderHeader != null)
                {
                    orderHeader.StateCode = OrderStateCode.PAID;
                    orderHeader.IsPaid = true;
                    dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
        }
    }
}
