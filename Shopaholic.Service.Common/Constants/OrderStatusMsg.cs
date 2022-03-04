using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Service.Common.Constants
{
    public class OrderStatusMsg
    {
        public static readonly string CREATE = "未繳費";
        public static readonly string PAID = "已繳費";
        public static readonly string PICKUP = "訂單確認";
        public static readonly string SHIP = "已出貨";
        public static readonly string ARRIVED = "已送達";
        public static readonly string APPLY_RETURN = "申請退貨";
        public static readonly string CONFIRM_RETURN = "確認退貨";
        public static readonly string OVERDUE = "未繳費逾期";
        public static readonly string PICKUP_FAIL = "揀貨失敗";
        public static readonly string SHIP_FAIL = "送貨失敗";
        public static readonly string CANCEL = "訂單取消";
    }
}
