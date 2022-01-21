using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using Shopaholic.CMS.Common.Middlewares;
using Shopaholic.Entity.Models;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMvc()
    //取消json小駝峰式命名法
    .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null); ;

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "API Document", 
        Version = "v1",
        Description = "API Document For Shopaholic.CMS"
    });
    var basePath = PlatformServices.Default.Application.ApplicationBasePath;
    var fileName = typeof(Program).GetTypeInfo().Assembly.GetName().Name + ".xml";
    c.IncludeXmlComments(Path.Combine(basePath, fileName));
    c.EnableAnnotations();
});

builder.Services.AddDbContext<ShopaholicContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AWS")/*,
        providerOptions => { providerOptions.EnableRetryOnFailure(); }*/);
});

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IStorageService>(provider => new ImgurService(builder.Configuration.GetValue<string>("Imgur:ClientID"), 
    builder.Configuration.GetValue<string>("Imgur:ClientSecret")));
builder.Services.AddScoped<IWebFlowService, WebFlowService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint(
        // url: 需配合 SwaggerDoc 的 name。 "/swagger/{SwaggerDoc name}/swagger.json"
        // name: 用於 Swagger UI 右上角選擇不同版本的 SwaggerDocument 顯示名稱使用。
        url: "/swagger/v1/swagger.json",
        name: "Shopaholic.CMS api v1"
    );
    c.RoutePrefix = "swagger";
});

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();