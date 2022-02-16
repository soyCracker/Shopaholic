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

        public OrderSearchResDTO SearchOrder(string searchStr, int page, int pageSize)
        {
            using(dbContext)
            {
                searchStr = string.IsNullOrEmpty(searchStr) ? "" : searchStr;
                List<OrderHeader> orderHeaders = dbContext.OrderHeaders.Where(x => (x.Id.Contains(searchStr) 
                    || x.ShipNumber.Contains(searchStr)
                    || x.Status.ToString().Contains(searchStr) 
                    || TimeUtil.DateTimeToYYYYMMdd(x.CreateTime, TimeUtil.yyyyMMddddFormat).Contains(searchStr)))
                    .OrderByDescending(y => y.CreateTime).Skip((page-1) * pageSize).Take(pageSize).Include(c => c.OrderDetail)
                    .ToList();

                List<OrderHeaderDTO> orderHeaderDTOs = new List<OrderHeaderDTO>();
                foreach (var order in orderHeaders)
                {
                    OrderHeaderDTO orderHeaderDTO = new OrderHeaderDTO
                    {
                        Id = order.Id,
                        UserId = order.UserId,
                        ShipNumber = order.ShipNumber,
                        Status = order.Status,
                        FailCode = order.FailCode,
                        CreateTime = order.CreateTime,
                        UpdateTime = order.UpdateTime
                    };
                    orderHeaderDTOs.Add(orderHeaderDTO);
                }

                int count = orderHeaderDTOs.Count();
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
