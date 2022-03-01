using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Web.Model.Requests
{
    public class AuthLoginReq
    {
        public string AccessToken { get; set; }
        public string Uid { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public bool EmailVerified { get; set; }
        public string PhotoURL { get; set; }
        public bool IsAnonymous { get; set; }
    }
}
