using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Shopaholic.Entity.Models;
using Shopaholic.Service.Common.Middlewares;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Services;
using Shopaholic.Web.Common.Configure;
using Shopaholic.Web.Common.Factory;
using StackExchange.Redis;
using System.Text.Encodings.Web;
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
builder.Services.AddScoped<IAuthService, MicrosoftAuthService>();
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
builder.Services.SetAuth(factory.GetEnvir());

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

