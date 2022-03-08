using Shopaholic.Util.Utilities;
using Microsoft.EntityFrameworkCore;
using Shopaholic.Entity.Models;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Model.Moels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlTypes;
using Shopaholic.Service.Common.Constants;
using Shopaholic.Web.Model.Requests;

namespace Shopaholic.Service.Services
{
    public class OrderService : IOrderService
    {
        private readonly ShopaholicContext dbContext;

        public OrderService(ShopaholicContext dbContext)
        {
            this.dbContext = dbContext;
        }      

        public OrderSearchResDTO SearchOrder(string searchStr, int page, int pageSize, string beginTime, string endTime)
        {
            using(dbContext)
            {
                DateTime searchBegin = string.IsNullOrEmpty(beginTime)?SqlDateTime.MinValue.Value:TimeUtil.StrToLocalDateTime(beginTime);
                DateTime searchEnd = string.IsNullOrEmpty(endTime)?SqlDateTime.MaxValue.Value:TimeUtil.StrToLocalDateTime(endTime);

                searchStr = string.IsNullOrEmpty(searchStr) ? "" : searchStr;
                var filterData = dbContext.OrderHeaders.Where(x => ( (x.OrderId.Contains(searchStr)
                    || x.StateCode.ToString().Contains(searchStr))
                    && (x.CreateTime>=searchBegin 
                    && x.CreateTime<=searchEnd) ) );

                List<OrderHeader> orderHeaders = filterData.OrderByDescending(y => y.CreateTime)
                    .Skip((page-1) * pageSize).Take(pageSize).ToList();
                List<OrderHeaderDTO> orderHeaderDTOs = new List<OrderHeaderDTO>();
                foreach (var order in orderHeaders)
                {
                    OrderHeaderDTO orderHeaderDTO = new OrderHeaderDTO
                    {
                        OrderId = order.OrderId,
                        UserId = order.UserId,
                        IsFinish = order.IsFinish,
                        IsSent = order.IsSent,
                        Status = order.StateCode,
                        FailCode = order.FailCode,
                        FormatCreateTime = TimeUtil.DateTimeToFormatStr(order.CreateTime, TimeUtil.yyyyMMddhhmmssFormat),
                        StatusMsg = GetOrderStatus(order.StateCode, order.FailCode)
                    };
                    orderHeaderDTOs.Add(orderHeaderDTO);
                }

                int count = filterData.Count();
                OrderSearchResDTO orderSearchResDTO = new OrderSearchResDTO
                {
                    OrderHeaderDTOs = orderHeaderDTOs,
                    TotalPages = count % pageSize == 0 ? count / pageSize : count / pageSize + 1
                };
                return orderSearchResDTO;
            }
        }

        public OrderSearchResDTO SearchUserOrder(string email, string searchStr, int page, int pageSize, string beginTime, string endTime)
        {
            using (dbContext)
            {
                var user = dbContext.CustomerAccounts.SingleOrDefault(x => x.Email == email);

                DateTime searchBegin = string.IsNullOrEmpty(beginTime) ? SqlDateTime.MinValue.Value : TimeUtil.StrToLocalDateTime(beginTime);
                DateTime searchEnd = string.IsNullOrEmpty(endTime) ? SqlDateTime.MaxValue.Value : TimeUtil.StrToLocalDateTime(endTime).AddDays(1);

                searchStr = string.IsNullOrEmpty(searchStr) ? "" : searchStr;
                var filterData = dbContext.OrderHeaders.Where(x => x.UserId == user.AccountId && ((x.OrderId.Contains(searchStr)
                    || x.StateCode.ToString().Contains(searchStr))
                    && (x.CreateTime>=searchBegin
                    && x.CreateTime<=searchEnd)));

                List<OrderHeader> orderHeaders = filterData.OrderByDescending(y => y.CreateTime)
                    .Skip((page-1) * pageSize).Take(pageSize).ToList();
                List<OrderHeaderDTO> orderHeaderDTOs = new List<OrderHeaderDTO>();
                foreach (var order in orderHeaders)
                {
                    OrderHeaderDTO orderHeaderDTO = new OrderHeaderDTO
                    {
                        OrderId = order.OrderId,
                        UserId = order.UserId,
                        IsFinish = order.IsFinish,
                        IsSent = order.IsSent,
                        Status = order.StateCode,
                        FailCode = order.FailCode,
                        FormatCreateTime = TimeUtil.DateTimeToFormatStr(order.CreateTime, TimeUtil.yyyyMMddhhmmssFormat),
                        StatusMsg = GetOrderStatus(order.StateCode, order.FailCode)
                    };
                    orderHeaderDTOs.Add(orderHeaderDTO);
                }

                int count = filterData.Count();
                OrderSearchResDTO orderSearchResDTO = new OrderSearchResDTO
                {
                    OrderHeaderDTOs = orderHeaderDTOs,
                    TotalPages = count % pageSize == 0 ? count / pageSize : count / pageSize + 1
                };
                return orderSearchResDTO;
            }
        }

        private string GetOrderStatus(int statusCode, int? failCode)
        {
            if(statusCode==OrderStateCode.CREATE && failCode==OrderFailCode.COMMON)
            {
                return OrderStatusMsg.CREATE;
            }
            else if(statusCode==OrderStateCode.PAID && failCode==OrderFailCode.COMMON)
            {
                return OrderStatusMsg.PAID;
            }
            else if(statusCode == OrderStateCode.PICKUP && failCode==OrderFailCode.COMMON)
            {
                return OrderStatusMsg.PICKUP;
            }
            else if(statusCode==OrderStateCode.SENT && failCode==OrderFailCode.COMMON)
            {
                return OrderStatusMsg.SHIP;
            }
            else if(statusCode == OrderStateCode.ARRIVED && failCode==OrderFailCode.COMMON)
            {
                return OrderStatusMsg.ARRIVED;
            }
            else if( statusCode == OrderStateCode.ARRIVED && failCode == OrderFailCode.RETURN)
            {
                return OrderStatusMsg.APPLY_RETURN;
            }
            else if (statusCode == OrderStateCode.CONFIRM_RETURN && failCode == OrderFailCode.RETURN)
            {
                return OrderStatusMsg.CONFIRM_RETURN;
            }
            else if (statusCode == OrderStateCode.CREATE && failCode == OrderFailCode.OVERDUE)
            {
                return OrderStatusMsg.OVERDUE;
            }
            else if (statusCode == OrderStateCode.PICKUP && failCode == OrderFailCode.PICKUP_FAIL)
            {
                return OrderStatusMsg.PICKUP_FAIL;
            }
            else if (statusCode == OrderStateCode.SENT && failCode == OrderFailCode.SHIP_FAIL)
            {
                return OrderStatusMsg.SHIP_FAIL;
            }
            else if (failCode == OrderFailCode.UN_ARRIVE_CANCEL)
            {
                return OrderStatusMsg.CANCEL;
            }
            return "確認中";
        }

        public bool ApplyReturn(string orderId)
        {
            using (dbContext)
            {
                var order = dbContext.OrderHeaders.SingleOrDefault(x => x.OrderId==orderId);
                if(order.StateCode==OrderStateCode.ARRIVED)
                {
                    order.FailCode = OrderFailCode.RETURN;
                    dbContext.SaveChanges();
                    return true;
                }
                else if(order.StateCode==OrderStateCode.CREATE || order.StateCode==OrderStateCode.PAID 
                    || order.StateCode==OrderStateCode.PICKUP || order.StateCode==OrderStateCode.SENT)
                {
                    order.FailCode=OrderFailCode.UN_ARRIVE_CANCEL;
                    dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
               
        }

        public OrderAllDataDTO GetOrderData(string orderId)
        {
            using(dbContext)
            {
                var orderHeader = dbContext.OrderHeaders.SingleOrDefault(x => x.OrderId==orderId);
                var shipment = dbContext.Shipments.SingleOrDefault(x => x.OrderId==orderId);
                List<OrderDetail> orderDetails = dbContext.OrderDetails.Where(x => x.OrderId==orderId).ToList();
                List<OrderDetailDTO> detailDtoList = new List<OrderDetailDTO>();
                foreach (var orderDetail in orderDetails)
                {
                    detailDtoList.Add(new OrderDetailDTO
                    {
                        OrderId = orderDetail.OrderId,
                        ProductId = orderDetail.ProductId,
                        Quantity = orderDetail.Quantity,
                        RealQuantity = orderDetail.RealQuantity,
                        IsBroken = orderDetail.IsBroken,
                        BrokenQuantity = orderDetail.BrokenQuantity,
                        CreateTime = orderDetail.CreateTime,
                        UpdateTime = orderDetail.UpdateTime,
                        ProductName = dbContext.Products.SingleOrDefault(x => x.Id==orderDetail.ProductId).Name
                    });
                }
                OrderAllDataDTO orderAllDataDTO = new OrderAllDataDTO
                {
                    OrderId = orderId,
                    UserId = orderHeader.UserId,
                    Status = orderHeader.StateCode,
                    FailCode = orderHeader.FailCode,
                    IsFinish = orderHeader.IsFinish,
                    IsSent = orderHeader.IsSent,
                    ShipNumber = shipment.ShipNumber,
                    FormatCreateTime = TimeUtil.DateTimeToFormatStr(orderHeader.CreateTime, TimeUtil.yyyyMMddhhmmssFormat),
                    StatusMsg = GetOrderStatus(orderHeader.StateCode, orderHeader.FailCode),
                    ReceiveMan = shipment.ReceiveMan,
                    Phone = shipment.Phone,
                    Address = shipment.Address,
                    Email = shipment.Email,
                    Details = detailDtoList
                };
                return orderAllDataDTO;
            }
        }

        public bool PickupConfirm(string orderId)
        {
            using (dbContext)
            {
                var order = dbContext.OrderHeaders.SingleOrDefault(x => x.OrderId==orderId);
                if(order!=null)
                {
                    if ((order.StateCode==OrderStateCode.CREATE || order.StateCode==OrderStateCode.PAID)
                        && order.FailCode==OrderFailCode.COMMON)
                    {
                        order.StateCode = OrderStateCode.PICKUP;
                        dbContext.SaveChanges();
                        return true;
                    }                                                        
                }
                return false;
            }
        }

        public bool ReturnConfirm(string orderId)
        {
            using (dbContext)
            {
                var order = dbContext.OrderHeaders.SingleOrDefault(x => x.OrderId==orderId);
                if (order!=null)
                {
                    if (order.StateCode==OrderStateCode.ARRIVED && order.FailCode==OrderFailCode.RETURN)
                    {
                        order.StateCode = OrderStateCode.CONFIRM_RETURN;
                        dbContext.SaveChanges();
                        return true;
                    }                                       
                }
                return false;
            }
        }

        public bool ForceFinish(string orderId)
        {
            using (dbContext)
            {
                var order = dbContext.OrderHeaders.SingleOrDefault(x => x.OrderId==orderId);
                if (order!=null)
                {
                    if (!(bool)order.IsFinish)
                    {
                        order.IsFinish = true;
                        dbContext.SaveChanges();
                        return true;
                    }                                    
                }
                return false;
            }
        }
    }
}
