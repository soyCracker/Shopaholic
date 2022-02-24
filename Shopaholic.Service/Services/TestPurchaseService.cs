using Shopaholic.Util.Utilities;
using Shopaholic.Entity.Models;
using Shopaholic.Service.Common.Business;
using Shopaholic.Service.Interfaces;
using Shopaholic.Web.Model.Requests;
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

        public string CreateOrder(PurchaseReq req)
        {
            using(dbContext)
            {
                string orderId = OrderBusiness.CreateOrder(dbContext, req);
                dbContext.SaveChanges();
                return orderId;
            }
        }      

        public bool Pay(int price)
        {
            return true;
        }
    }
}
