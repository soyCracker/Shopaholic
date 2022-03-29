using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shopaholic.Entity.Models;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Services;
using Shopaholic.Web.Common.Factory;
using Shopaholic.Web.Common.Middlewares;
using StackExchange.Redis;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

// ������������
//string envirMode = "DEV";
string envirMode = "AWS";
EnvirFactory envirFactory = new EnvirFactory(builder.Configuration, envirMode);

// IHttpClientFactory
builder.Services.AddHttpClient();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IWebFlowService, WebFlowService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAuthService, FirebaseGoogleAuthService>();
builder.Services.AddScoped<ICartService, ShoppingCartService>();
builder.Services.AddScoped<LinePayPurchaseService>();
builder.Services.AddScoped<EcPayPurchaseService>();
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(envirFactory.GetEnvir().GetReddisConnStr()));
// �ۭq HtmlEcoder �N�򥻩ԤB�r���P�������r���ǤJ���\�d�򤣰���X
builder.Services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs }));
builder.Services.AddScoped(provider => envirFactory);

builder.Services.AddDbContext<ShopaholicContext>(options =>
{
    options.UseSqlServer(envirFactory.GetEnvir().GetDbConnStr(),
        providerOptions => { providerOptions.EnableRetryOnFailure(); });
});


// Firebase Authentication
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://securetoken.google.com/shopaholic-39229";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "https://securetoken.google.com/shopaholic-39229",
            ValidateAudience = true,
            ValidAudience = "shopaholic-39229",
            ValidateLifetime = true
        };
        // ����������Cookie-based Authentication
    }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options=>
    {
        options.Events.OnRedirectToLogin = context =>
        {
            //��MVC��API���ҥ��Ѯɦ����P���欰
            if (Regex.IsMatch(context.Request.Path.Value, "/api/", RegexOptions.IgnoreCase))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return Task.CompletedTask;
            }
            context.Response.Redirect(new PathString(builder.Configuration.GetValue<string>("LoginUrl")));
            return Task.CompletedTask;
        };        
    });

// ASP.NET Data Protection�A�x�s��redis�A�ѨMload balancer��Cookie-based Auth��server���D
using (ServiceProvider serviceProvider = builder.Services.BuildServiceProvider())
{
    // nuget Microsoft.AspNetCore.DataProtection.StackExchangeRedis
    builder.Services.AddDataProtection()
        .PersistKeysToStackExchangeRedis(serviceProvider.GetRequiredService<IConnectionMultiplexer>(), "DataProtectionKeys");
}

// Add services to the container.
builder.Services.AddMvc()
    .AddJsonOptions(opts =>
    {
        //����json�p�m�p���R�W�k
        opts.JsonSerializerOptions.PropertyNamingPolicy = null;
        //���\�򥻩ԤB�^��Τ�������r������r��
        opts.JsonSerializerOptions.Encoder =
            JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs);
    });

// �]�wPORT
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

// ���oIP
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
    ForwardedHeaders.XForwardedProto
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
