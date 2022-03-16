// See https://aka.ms/new-console-template for more information
using Shopaholic.Util.Utilities;
using Microsoft.EntityFrameworkCore;
using Shopaholic.Base.Console.Class;
using Shopaholic.Entity.Models;
using Shopaholic.Service.Common.Constants;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Model.Moels;
using Shopaholic.Service.Services;
using Shopaholic.Web.Model.Requests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

Console.WriteLine("Hello, World!");
//var conStr = "Server=.\\SQLEXPRESS;Database=Shopaholic;Trusted_Connection=True;";

var conStr = "Data Source=database-1.cjlz3wjjlt1i.ap-northeast-1.rds.amazonaws.com;Database=Shopaholic;Persist Security Info=True;User ID=admin;Password=741852963;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;";



using (var services = new ServiceCollection()
            .AddHttpClient()
            .AddLogging(config => {
                config.AddConsole();
                config.SetMinimumLevel(LogLevel.Debug);
            })
            .BuildServiceProvider())
{
    var logger = services.GetRequiredService<ILogger<WriteTestDataClass>>();

    IHttpClientFactory httpClientFactory = services.GetRequiredService<IHttpClientFactory>();

    WriteTestDataClass writeTestDataClass = new WriteTestDataClass(logger);

    //// 寫入測試類別
    //writeTestDataClass.WriteCategory(conStr);

    //// 寫入測試商品
    //writeTestDataClass.WriteProduct(conStr);

    //// 寫入正式類別、商品
    //var writeFormalDataLogger = services.GetRequiredService<ILogger<WriteFormalDataClass>>();
    //WriteFormalDataClass writeFormalDataClass = new WriteFormalDataClass(writeFormalDataLogger);
    //writeFormalDataClass.WriteCategory(conStr);
    //writeFormalDataClass.WriteFoodProduct(conStr);
    //writeFormalDataClass.WritePhoneProduct(conStr);
    //writeFormalDataClass.WriteGameProduct(conStr);
    //writeFormalDataClass.WriteDecorateProduct(conStr);
    //writeFormalDataClass.WriteDailyUseProduct(conStr);

    //// 寫入測試flow
    writeTestDataClass.WriteFlow(conStr);

    ////// 寫入測試會員
    //writeTestDataClass.WriteAuth(conStr);

    ////// 寫入測試訂單
    //writeTestDataClass.WriteOrder(conStr);

    //// LinePay測試
    //LinePayTestClass linePayTestClass = new LinePayTestClass(services.GetRequiredService<ILogger<LinePayTestClass>>(), httpClientFactory);
    //await linePayTestClass.LinePayPost();
}
