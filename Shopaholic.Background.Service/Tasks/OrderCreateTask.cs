using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shopaholic.Entity.Models;
using Shopaholic.Service.Common.Constants;
using Shopaholic.Util.Utilities;
using Shopaholic.Web.Model.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Background.Service.Tasks
{
    public class OrderCreateTask
    {
        private readonly ILogger logger;
        private static readonly object orderLock = new object();
        private readonly IServiceScopeFactory scopeFactory;

        public OrderCreateTask(ILogger<OrderCreateTask> logger, IServiceScopeFactory scopeFactory)
        {
            this.logger = logger;
            this.scopeFactory = scopeFactory;
        }

        public string Start(PurchaseOrderCreateReq req) 
        {
            lock (orderLock)
            {
                // use dbContext inside singleton
                using (var scope = scopeFactory.CreateScope())
                {
                    string todayStr = TimeUtil.GetUtcDateTime().ToString(TimeUtil.yyyyMMdd_02);
                    string targetOrderId = todayStr + OrderNumberFormat.ORDERNUMBER_INIT_SEQ;
                    var dbContext = scope.ServiceProvider.GetRequiredService<ShopaholicContext>();
                    if (dbContext.OrderHeaders.Count()>0)
                    {
                        string maxOrderIdInDb = dbContext.OrderHeaders.OrderByDescending(x => x.OrderId).First().OrderId;
                        logger.LogInformation("OrderCreateTask Start() maxOrderIdInDb=" + maxOrderIdInDb);
                        if (!string.IsNullOrEmpty(maxOrderIdInDb) && maxOrderIdInDb.Substring(0, 8)==todayStr)
                        {
                            targetOrderId = todayStr + StringUtil.AddZeroPadLeft(
                                    (int.Parse(maxOrderIdInDb.Substring(8, 6).TrimStart('0')) + 1).ToString(), 6);
                        }
                    }
                    logger.LogInformation("OrderCreateTask Start() targetOrderId=" + targetOrderId);
                    dbContext.OrderHeaders.Add(GetOrderHeader(targetOrderId, req));
                    dbContext.Shipments.Add(GetShipment(targetOrderId, req));
                    dbContext.OrderDetails.AddRange(GetOrderDetailList(targetOrderId, req));
                    dbContext.SaveChanges();
                    return targetOrderId;
                }
            }                          
        }

        private OrderHeader GetOrderHeader(string orderId, PurchaseOrderCreateReq req)
        {
            return new OrderHeader
            {
                OrderId = orderId,
                UserId = req.UserId,
                StateCode = 0,
                OrderTypeCode = req.OrderTypeCode
            };
        }

        private Shipment GetShipment(string orderId, PurchaseOrderCreateReq req)
        {
            return new Shipment
            {
                OrderId = orderId,
                ReceiveMan = req.ReceiveMan,
                Phone = req.Phone,
                Address = req.Address,
                Email = req.Email
            };
        }

        private List<OrderDetail> GetOrderDetailList(string orderId, PurchaseOrderCreateReq req)
        {
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            foreach (var detail in req.ProductList)
            {
                orderDetails.Add(new OrderDetail
                {
                    OrderId = orderId,
                    ProductId = detail.ProductId,
                    Quantity = detail.Quantity,
                    CurrentPrice = detail.CurrentPrice
                });
            }
            return orderDetails;
        }

        public void Test()
        {
            logger.LogInformation("OrderCreateTask Test()");
        }
    }
}
