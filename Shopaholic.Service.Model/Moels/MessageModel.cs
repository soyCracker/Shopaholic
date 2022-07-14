using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Service.Model.Moels
{
    public class MessageModel<T>
    {
        public int StatusCode { get; set; } = 200;

        public bool Success { get; set; } = true;

        public string Msg { get; set; } = "";

        public T Data { get; set; }
    }
}
