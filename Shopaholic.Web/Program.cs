using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Shopaholic.Entity.Models;
using Shopaholic.Service.Common.Middlewares;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Services;
using Shopaholic.Web.Common.Factory;
using StackExchange.Redis;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

// Add MVC一定要放在前面，不然放到雲端會無法載入css之類的東西
builder.Services.AddMvc()
    .AddJsonOptions(opts =>
    {
        //取消json小駝峰式命名法
        opts.JsonSerializerOptions.PropertyNamingPolicy = null;
        //允許基本拉丁英文及中日韓文字維持原字元
        opts.JsonSerializerOptions.Encoder =
            JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs);
    });

//決定執行環境
EnvirFactory factory = new EnvirFactory();

//IHttpClientFactory
builder.Services.AddHttpClient();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IWebFlowService, WebFlowService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAuthService, FirebaseGoogleAuthService>();
builder.Services.AddScoped<ICartService, ShoppingCartService>();
builder.Services.AddScoped<IPopularService, PopularService>();
builder.Services.AddScoped<IPurchaseService, LinePayPurchaseService>();
builder.Services.AddScoped<IPurchaseService, EcPayPurchaseService>();
builder.Services.AddSingleton(provider => factory.GetEnvir());
//redis singleton DI
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(factory.GetEnvir().GetReddisConnStr()));
//自訂 HtmlEcoder 將基本拉丁字元與中日韓字元納入允許範圍不做轉碼
builder.Services.AddSingleton(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs }));
builder.Services.AddDbContext<ShopaholicContext>(options =>
{
    options.UseSqlServer(factory.GetEnvir().GetDbConnStr(),
        providerOptions => { providerOptions.EnableRetryOnFailure(); });
});

//builder.Services.AddOidcStateDataFormatterCache();
builder.Services
    .AddAuthentication(options =>
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
            context.Response.Redirect(new PathString(factory.GetEnvir().GetLoginUrl()));
            return Task.CompletedTask;
        };
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
        //登入後過期時間內没有進行操作就會過期;false有操作還是會過期
        options.SlidingExpiration = true;
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
    //    options.CallbackPath = "/Auth/google-callback";
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
    .AddMicrosoftAccount("Microsoft", option =>
    {
        option.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        option.ClientId = factory.GetEnvir().GetMsClientId();
        option.ClientSecret = factory.GetEnvir().GetMsClientSecret();
        //option.CallbackPath = "/Auth/signin-microsoft";
    });

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None;
});

// ASP.NET Data Protection，儲存於redis，解決load balancer的Cookie-based Auth跨server問題
using (ServiceProvider serviceProvider = builder.Services.BuildServiceProvider())
{
    // nuget Microsoft.AspNetCore.DataProtection.StackExchangeRedis
    builder.Services.AddDataProtection()
        .PersistKeysToStackExchangeRedis(serviceProvider.GetRequiredService<IConnectionMultiplexer>(), "DataProtectionKeys");
}

// 設定PORT
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(12970); // to listen for incoming http connection on port 5001
    //options.ListenAnyIP(80);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

// 搭配proxy server和load balancer，header轉送
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
    ForwardedHeaders.XForwardedProto
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
