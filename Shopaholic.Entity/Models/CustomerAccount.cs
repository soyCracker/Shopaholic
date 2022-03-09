using System;
using System.Collections.Generic;

namespace Shopaholic.Entity.Models
{
    public partial class CustomerAccount
    {
        public int Id { get; set; }
        public string AccountId { get; set; } = null!;
        public string? DisplayName { get; set; }
        public string Email { get; set; } = null!;
        public bool EmailVerified { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Type { get; set; }
    }
}
