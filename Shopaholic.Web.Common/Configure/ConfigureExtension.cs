using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Shopaholic.Service.Common.Environment;
using System.Net;
using System.Text.RegularExpressions;

namespace Shopaholic.Web.Common.Configure
{
    public static class ConfigureExtension
    {
        public static void SetAuth(this IServiceCollection sc, IEnvironment env)
        {
            sc.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "Microsoft";
            })
            //網站本身的Cookie - based Authentication
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    //讓MVC及API驗證失敗時有不同的行為
                    if (Regex.IsMatch(context.Request.Path.Value, "/api/", RegexOptions.IgnoreCase))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        return Task.CompletedTask;
                    }
                    context.Response.Redirect(new PathString(env.GetLoginUrl()));
                    return Task.CompletedTask;
                };
                // ExpireTimeSpan與Cookie.MaxAge都要設定
                options.ExpireTimeSpan = TimeSpan.FromHours(6);
                options.Cookie.MaxAge = options.ExpireTimeSpan;
                //登入後過期時間內没有進行操作就會過期;false有操作還是會過期
                options.SlidingExpiration = false;
            })
            //.AddOpenIdConnect("Google", "Google", options =>
            //{
            //    options.Authority = "https://accounts.google.com";
            //    options.ClientId = "198239108223-pkjlt5kerinovh2du3npf514vfvtrrlp.apps.googleusercontent.com";
            //    options.ClientSecret = "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";
            //    options.ResponseType = "code";
            //    options.Scope.Add("openid");
            //    options.Scope.Add("profile");
            //    options.SaveTokens = true;
            //});
            //.AddGoogle("Google", options =>
            //{
            //    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.ClientId = "198239108223-pkjlt5kerinovh2du3npf514vfvtrrlp.apps.googleusercontent.com";
            //    options.ClientSecret = "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";
            //});
            //.AddJwtBearer("Firebase", option =>
            //{
            //    option.Authority = factory.GetEnvir().GetFirebaseUrl();
            //    option.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidIssuer = factory.GetEnvir().GetFirebaseUrl(),
            //        ValidateAudience = true,
            //        ValidAudience = factory.GetEnvir().GetFirebaseID(),
            //        ValidateLifetime = true,
            //        ClockSkew = TimeSpan.Zero
            //    };
            //})
            .AddMicrosoftAccount("Microsoft", options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.ClientId = env.GetMsClientId();
                options.ClientSecret = env.GetMsClientSecret();
            });
        }
    }
}
