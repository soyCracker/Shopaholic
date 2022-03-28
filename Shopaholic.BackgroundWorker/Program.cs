using Microsoft.EntityFrameworkCore;
using Shopaholic.Background.Service.Servicces;
using Shopaholic.Background.Service.Tasks;
using Shopaholic.Entity.Models;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ShopaholicContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DEV"),
        providerOptions => { providerOptions.EnableRetryOnFailure(); });
});

// Add services to the container.
builder.Services.AddHostedService<OrderHostedService>();
builder.Services.AddSingleton<OrderCreateTask>();

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
    options.ListenAnyIP(14444);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
