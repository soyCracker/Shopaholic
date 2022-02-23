using ClassLibrary.Utilities;
using Microsoft.EntityFrameworkCore;
using Shopaholic.Entity.Models;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Model.Moels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Service.Services
{
    public class OrderService : IOrderService
    {
        private readonly ShopaholicContext dbContext;

        public OrderService(ShopaholicContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public OrderSearchResDTO SearchOrder(string searchStr, int page, int pageSize, DateTime beginTime, DateTime endTime)
        {
            using(dbContext)
            {
                searchStr = string.IsNullOrEmpty(searchStr) ? "" : searchStr;
                var filterData = dbContext.OrderHeaders.Where(x => (x.OrderId.Contains(searchStr)
                    || x.StateCode.ToString().Contains(searchStr)
                    && (x.CreateTime>=beginTime && x.CreateTime<=endTime)));



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
                        FormatCreateTime = TimeUtil.DateTimeToFormatStr(order.CreateTime, TimeUtil.yyyyMMddhhmmssFormat)
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
    }
}
