using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.CMS.Model.Requests
{
    public class ProductGetListRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
    }
}
