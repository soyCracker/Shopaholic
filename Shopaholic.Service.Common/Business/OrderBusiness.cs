using Shopaholic.Entity.Models;
using Shopaholic.Service.Common.Constants;
using Shopaholic.Util.Utilities;
using Shopaholic.Web.Model.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Service.Common.Business
{
    public class OrderBusiness
    {
        private static readonly object orderLock = new object();

        public static string CreateOrder(ShopaholicContext dbContext, PurchaseReq req)
        {
            string targetOrderId = "";
            lock (orderLock)
            {
                DateTime today = TimeUtil.GetLocalTodayDate();
                targetOrderId = TimeUtil.DateTimeToFormatStr(today, TimeUtil.yyyyMMddFormat_02)
                        + OrderNumberFormat.ORDERNUMBER_INIT_SEQ;
                if (dbContext.OrderHeaders.Count()>0)
                {
                    string maxOrderId = dbContext.OrderHeaders.OrderByDescending(x=>x.OrderId).First().OrderId;
                    if(maxOrderId.Substring(0, 8)==TimeUtil.DateTimeToFormatStr(today, TimeUtil.yyyyMMddFormat_02))
                    {
                        // 避免執行過程中剛好過一天，用最大OrderId的日期
                        targetOrderId = maxOrderId.Substring(0, 8) 
                            + StringUtil.AddZeroPadLeft(
                                (int.Parse(maxOrderId.Substring(8, 6).TrimStart('0')) + 1).ToString(), 6);
                    }
                }

                OrderHeader orderHeader = new OrderHeader
                {
                    OrderId = targetOrderId,
                    UserId = req.UserId,
                    StateCode = 0,
                    OrderTypeCode = req.OrderTypeCode
                };
                dbContext.OrderHeaders.Add(orderHeader);

                Shipment shipment = new Shipment
                {
                    OrderId = targetOrderId,
                    ReceiveMan = req.ReceiveMan,
                    Phone = req.Phone,
                    Address = req.Address,
                    Email = req.Email
                };
                dbContext.Shipments.Add(shipment);

                List<OrderDetail> orderDetails = new List<OrderDetail>();
                foreach(var detail in req.ProductList)
                {
                    orderDetails.Add(new OrderDetail
                    {
                        OrderId = targetOrderId,
                        ProductId = detail.ProductId,
                        Quantity = detail.Quantity,
                        CurrentPrice = detail.CurrentPrice
                    });
                }
                dbContext.OrderDetails.AddRange(orderDetails);                               
            }

            return targetOrderId;
        }
    }
}
