using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Service.Interfaces
{
    public interface IAuthService
    {
        void UpdateUser(string accessToken, string uid, string displayName, string email,
            bool emailVerified, string photoUrl, bool isAnonymous);

        bool ChkExist(string uid, string email);
    }
}
