﻿using Shopaholic.Service.Model.Moels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Service.Interfaces
{
    public interface IOrderService
    {
        OrderSearchResDTO SearchOrder(string searchStr, int page, int pageSize, string beginTime, string endTime);

        OrderSearchResDTO SearchUserOrder(string email, string searchStr, int page, int pageSize, string beginTime, string endTime);

        OrderAllDataDTO GetOrderData(string orderId);

        bool ApplyReturn(string orderId);

        bool PickupConfirm(string orderId);

        bool ReturnConfirm(string orderId);

        bool ForceFinish(string orderId);
    }
}
