using System;
using System.Collections.Generic;

#nullable disable

namespace Shopaholic.Entity.Models
{
    public partial class WebFlow
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public string Enter { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
