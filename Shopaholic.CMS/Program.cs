using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Shopaholic.CMS.Common.Factory;
using Shopaholic.Entity.Models;
using Shopaholic.Service.Common.Middlewares;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Services;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

// 切換執行環境
EnvirFactory factory = new EnvirFactory();

// Add services to the container.
builder.Services.AddMvc()
    .AddJsonOptions(opts =>
    {
        //取消json小駝峰式命名法
        opts.JsonSerializerOptions.PropertyNamingPolicy = null;
        //允許基本拉丁英文及中日韓文字維持原字元
        opts.JsonSerializerOptions.Encoder =
            JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs);
    });

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Document",
        Version = "v1",
        Description = "API Document For Shopaholic.CMS"
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "myAllowSpecificOrigins",
                      builder =>
                      {
                          builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                      });
});

builder.Services.AddDbContext<ShopaholicContext>(options =>
{
    options.UseSqlServer(factory.GetEnvir().GetDbConnStr());
});
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IStorageService>(provider => new ImgurService(factory.GetEnvir().GetImgurClientID(),
    factory.GetEnvir().GetImgurClientSecret()));
builder.Services.AddScoped<IWebFlowService, WebFlowService>();
builder.Services.AddScoped<IOrderService, OrderService>();
// 自訂 HtmlEcoder 將基本拉丁字元與中日韓字元納入允許範圍不做轉碼
builder.Services.AddSingleton(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs }));
//加入EnvirFactory DI
builder.Services.AddSingleton(provider => factory);
// set port
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(12770); // to listen for incoming http connection on port 5001
});




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
    // 需配合 SwaggerDoc 的 name。 "/swagger/{SwaggerDoc name}/swagger.json"
    // 用於 Swagger UI 右上角選擇不同版本的 SwaggerDocument 顯示名稱使用。
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shopaholic.CMS api v1");
    c.RoutePrefix = "swagger";
});

app.UseRouting();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();