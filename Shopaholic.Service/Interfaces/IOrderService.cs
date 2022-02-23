using Shopaholic.Service.Model.Moels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Service.Interfaces
{
    public interface IOrderService
    {
        OrderSearchResDTO SearchOrder(string searchStr, int page, int pageSize, DateTime beginTime, DateTime endTime);
    }
}
