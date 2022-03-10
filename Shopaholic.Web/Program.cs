using AspNetCore.Firebase.Authentication.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shopaholic.Entity.Models;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Services;
using Shopaholic.Web.Common.Middlewares;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ShopaholicContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AWS")/*,
        providerOptions => { providerOptions.EnableRetryOnFailure(); }*/);
});

builder.Services.AddHttpClient();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IStorageService>(provider => new ImgurService(builder.Configuration.GetValue<string>("Imgur:ClientID"),
    builder.Configuration.GetValue<string>("Imgur:ClientSecret")));
builder.Services.AddScoped<IWebFlowService, WebFlowService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAuthService, FirebaseGoogleAuthService>();
builder.Services.AddScoped<ICartService, ShoppingCartService>();
builder.Services.AddScoped<IPurchaseService, LinePayPurchaseService>();

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
    }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options=>
    {
        options.LoginPath = new PathString(builder.Configuration.GetValue<string>("LoginUrl"));
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.OnStarting(() =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                return Task.CompletedTask;
            });
            return Task.CompletedTask;
        };
    });

// Add services to the container.
builder.Services.AddMvc(options => { options.EnableEndpointRouting = false; })
    //取消json小駝峰式命名法
    .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(12970); // to listen for incoming http connection on port 5001
    options.ListenAnyIP(12971, configure => configure.UseHttps()); // to listen for incoming https connection on port 7001
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

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
