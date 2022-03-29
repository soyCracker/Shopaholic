using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using Shopaholic.CMS.Common.Tools;
using Shopaholic.Entity.Models;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Services;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

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
    options.UseSqlServer(builder.Configuration.GetConnectionString("AWS")/*,
        providerOptions => { providerOptions.EnableRetryOnFailure(); }*/);
});

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IStorageService>(provider => new ImgurService(builder.Configuration.GetValue<string>("Imgur:ClientID"), 
    builder.Configuration.GetValue<string>("Imgur:ClientSecret")));
builder.Services.AddScoped<IWebFlowService, WebFlowService>();
builder.Services.AddScoped<IOrderService, OrderService>();
// �ۭq HtmlEcoder �N�򥻩ԤB�r���P�������r���ǤJ���\�d�򤣰���X
builder.Services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs }));


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
    c.SwaggerEndpoint(
        // url: �ݰt�X SwaggerDoc �� name�C "/swagger/{SwaggerDoc name}/swagger.json"
        // name: �Ω� Swagger UI �k�W����ܤ��P������ SwaggerDocument ��ܦW�٨ϥΡC
        url: "/swagger/v1/swagger.json",
        name: "Shopaholic.CMS api v1"
    );
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