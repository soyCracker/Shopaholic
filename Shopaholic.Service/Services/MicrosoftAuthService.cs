﻿using Shopaholic.Entity.Models;
using Shopaholic.Service.Interfaces;

namespace Shopaholic.Service.Services
{
    public class MicrosoftAuthService : IAuthService
    {
        private readonly ShopaholicContext dbContext;
        private readonly string userType = "Microsoft";

        public MicrosoftAuthService(ShopaholicContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public bool ChkExist(string uid, string email)
        {
            using (dbContext)
            {
                var user = dbContext.CustomerAccounts.SingleOrDefault(x => x.Email == email);
                if (user == null)
                {
                    return false;
                }
                return true;
            }
        }

        public void UpdateUser(string accessToken, string uid, string displayName, string email, bool emailVerified, string photoUrl, bool isAnonymous)
        {
            using (dbContext)
            {
                var user = dbContext.CustomerAccounts.SingleOrDefault(x => x.Email == email && x.Type == userType);
                if (user == null)
                {
                    dbContext.CustomerAccounts.Add(new CustomerAccount
                    {
                        AccountId = Guid.NewGuid().ToString(),
                        DisplayName = displayName,
                        Email = email,
                        EmailVerified = emailVerified,
                        PhotoUrl = photoUrl,
                        Type = userType
                    });
                }
                else
                {
                    user.DisplayName = displayName;
                    user.EmailVerified = emailVerified;
                    user.PhotoUrl = photoUrl;
                }
                dbContext.SaveChanges();
            }
        }
    }
}
