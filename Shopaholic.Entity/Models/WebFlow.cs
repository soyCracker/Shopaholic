using System;
using System.Collections.Generic;

namespace Shopaholic.Entity.Models
{
    public partial class WebFlow
    {
        public int Id { get; set; }
        public string? Ip { get; set; }
        public string Enter { get; set; } = null!;
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
