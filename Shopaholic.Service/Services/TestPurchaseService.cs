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
using Shopaholic.Service.Common.Constants;

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
                req.UserId = dbContext.CustomerAccounts.SingleOrDefault(x => x.Email==req.Email).AccountId;
                var carts = dbContext.ShoppingCarts.Where(x => x.AccountId==req.UserId).ToList();
                List<PurchaseProductModel> ProductList = new List<PurchaseProductModel>();
                foreach (var cart in carts)
                {
                    var product = dbContext.Products.SingleOrDefault(p => p.Id==cart.ProductId);
                    ProductList.Add(new PurchaseProductModel
                    {
                        ProductId = cart.ProductId,
                        Quantity = cart.Quantity,
                        CurrentPrice = product.Price
                    });
                }
                req.ProductList = ProductList;

                //// TODO 未來要改成獨立Server建立訂單編號
                string orderId = OrderBusiness.CreateOrder(dbContext, req);

                dbContext.ShoppingCarts.RemoveRange(carts);

                dbContext.SaveChanges();
                return orderId;
            }
        }      

        public bool Pay(PayReq req)
        {
            using (dbContext)
            {
                req.UserId = dbContext.CustomerAccounts.SingleOrDefault(x => x.Email==req.Email).AccountId;
                var order = dbContext.OrderHeaders.SingleOrDefault(x => x.OrderId==req.OrderId);
                List<OrderDetail> orderDetails = dbContext.OrderDetails.Where(x => x.OrderId==req.OrderId).ToList();
                int totalPrice = 0;
                foreach (var detail in orderDetails)
                {
                    int price = dbContext.Products.SingleOrDefault(x => x.Id==detail.ProductId).Price;
                    totalPrice+=detail.Quantity*price;
                }

                //金流付款成功
                if (true)
                {
                    order.StateCode = OrderStateCode.PAID;
                    dbContext.SaveChanges();
                    return true;
                }
                return false;
            }                           
        }
    }
}
