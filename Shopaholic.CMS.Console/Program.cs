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


Console.WriteLine("Hello, World!");
var conStr = "Server=.\\SQLEXPRESS;Database=Shopaholic;Trusted_Connection=True;";

//var conStr = "Data Source=database-1.cjlz3wjjlt1i.ap-northeast-1.rds.amazonaws.com;Database=Shopaholic;Persist Security Info=True;User ID=admin;Password=741852963;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;";

//// 寫入測試類別
ICategoryService categoryService = new CategoryService(DBClass.GetDbContext(conStr));
categoryService.AddCategory("未分類");

//// 寫入測試商品
List<ProductDTO> products = new List<ProductDTO>();
IProductService productService = new ProductService(DBClass.GetDbContext(conStr));
int testCategoryId = DBClass.GetDbContext(conStr).Categories.SingleOrDefault(x => x.Name == "未分類").Id;
for (int i = 1; i <= 50; i++)
{
    products.Add(new ProductDTO
    {
        Name = "餅乾測試" + i.ToString().PadLeft(2, '0'),
        Description = "我的天啊",
        CategoryId = testCategoryId,
        Content = "我的天啊",
        Image = "https:////via.placeholder.com//1200//FFFFFF.png?text=1",
        Price = 9999,
        Stock = 8787,
        IsDelete = false
    });
    Console.WriteLine("Write " + i.ToString().PadLeft(2, '0') + " Success");
}
productService.AddProductRange(products);

// 寫入測試flow
List<FlowDTO> flowDtoList = new List<FlowDTO>();
IWebFlowService webFlowService = new WebFlowService(DBClass.GetDbContext(conStr));
for (DateTime date = DateTime.Now.Date.AddDays(-29); date <= DateTime.Now.Date; date = date.AddDays(1))
{
    int rndMax = Random.Shared.Next(5, 50);
    for (int r = 0; r<rndMax; r++)
    {
        flowDtoList.Add(new FlowDTO
        {
            Ip = "127.0.0.1",
            Enter = "Test Flow " + r.ToString().PadLeft(2, '0'),
            CreateTime = date
        });
        Console.WriteLine("Write " + r.ToString().PadLeft(2, '0') + " " + date + " Success");
    }
}
webFlowService.AddFlowRange(flowDtoList);

//List<Task<int>> tasks = new List<Task<int>>();
//TaskFactory factory = new TaskFactory();
//object orderLock = new object();
//tasks.Add(factory.StartNew<int>(() =>
//{
//    for (int i = 0; i < 10; i++)
//    {
//        IPurchaseService purchaseService = new TestPurchaseService(DBClass.GetDbContext(conStr));
//        List<PurchaseProductModel> purchaseProductList = new List<PurchaseProductModel>();
//        purchaseProductList.Add(new PurchaseProductModel
//        {
//            ProductId = 1,
//            Quantity = 5,
//            CurrentPrice = 777
//        });
//        PurchaseReq req = new PurchaseReq
//        {
//            OrderTypeCode = OrderTypeCode.TEST,
//            UserId = "TEST0000001GGG",
//            ReceiveMan = "TEST_MAN_GGGGGG52869416",
//            Email = "TESTTEST00000123456798@gmail.com.test",
//            Phone = "1000000000",
//            Address = "TEST_ADDRESS",
//            ProductList = purchaseProductList
//        };
//        lock (orderLock)
//            Console.WriteLine("OrderId: " + purchaseService.CreateOrder(req));
//    }
//    return 1;
//}));


//tasks.Add(factory.StartNew<int>(() =>
//{
//    for (int i = 0; i < 10; i++)
//    {
//        IPurchaseService purchaseService = new TestPurchaseService(DBClass.GetDbContext(conStr));
//        List<PurchaseProductModel> purchaseProductList = new List<PurchaseProductModel>();
//        purchaseProductList.Add(new PurchaseProductModel
//        {
//            ProductId = 1,
//            Quantity = 5,
//            CurrentPrice = 777
//        });

//        PurchaseReq req = new PurchaseReq
//        {
//            OrderTypeCode = OrderTypeCode.TEST,
//            UserId = "TEST0000001GGG",
//            ReceiveMan = "TEST_MAN_GGGGGG52869416",
//            Email = "TESTTEST00000123456798@gmail.com.test",
//            Phone = "1000000000",
//            Address = "TEST_ADDRESS",
//            ProductList = purchaseProductList
//        };
//        lock (orderLock)
//            Console.WriteLine("OrderId: " + purchaseService.CreateOrder(req));
//    }
//    return 2;
//}));

//foreach (var t in tasks)
//{
//    Console.WriteLine("Task:" + t.Result);
//}