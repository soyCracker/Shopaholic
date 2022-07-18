using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Web.Common.Interface
{
    public interface IEnvironment
    {
        string GetDbConnStr();

        string GetReddisConnStr();

        string GetLinePayApiUrl();

        string GetLinePayChannelSecret();

        string GetLinePayChannelId();

        string GetLinePayBaseUrl();

        string GetLinePayConfirmApi();

        string GetOrderIdCreateApi();

        string GetEcPayApi();

        string GetEcPayConfirmApi();

        string GetPayConfirmReturnPage();

        string GetEcPayMerchantID();

        string GetFirebaseID();

        string GetFirebaseUrl();

        string GetLoginUrl();
    }
}
