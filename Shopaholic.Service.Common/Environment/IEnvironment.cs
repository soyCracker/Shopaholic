namespace Shopaholic.Service.Common.Environment
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
        string GetImgurClientID();
        string GetImgurClientSecret();
        string FrontWebUrl();
        string CMSWebUrl();
        string GetMsClientId();
        string GetMsClientSecret();
    }
}
