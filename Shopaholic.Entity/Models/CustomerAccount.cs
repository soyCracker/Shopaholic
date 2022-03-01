using System;
using System.Collections.Generic;

#nullable disable

namespace Shopaholic.Entity.Models
{
    public partial class CustomerAccount
    {
        public int Id { get; set; }
        public string AccountId { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public bool EmailVerified { get; set; }
        public string PhotoUrl { get; set; }
        public string Type { get; set; }
    }
}
