using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.CMS.Model.Requests
{
    public class OrderSearchReq
    {
        [SwaggerSchema("搜索字串")]
        public string SearchStr { get; set; }
        [SwaggerSchema("頁數")]
        public int Page { get; set; }
        [SwaggerSchema("每頁數量")]
        public int PageSize { get; set; }
        [SwaggerSchema("搜索範圍開始日期")]
        public string BeginTime { get; set; }
        [SwaggerSchema("搜索範圍結束日期")]
        public string EndTime { get; set; }
    }
}
