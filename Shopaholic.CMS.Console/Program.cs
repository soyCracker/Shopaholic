// See https://aka.ms/new-console-template for more information
using Shopaholic.Entity.Models;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Services;

Console.WriteLine("Hello, World!");


// 寫入測試商品
/*
for(int i = 1; i <= 50; i++)
{
    IProductService productService = new ProductService(new ShopaholicContext { });
    productService.AddProduct("餅乾測試" + i.ToString().PadLeft(2, '0'), "我的天啊", 11, "我的天啊", null, 9999, 8787);
    Console.WriteLine("Write " + i.ToString().PadLeft(2, '0') + " Success");
}
*/

// 寫入測試flow
IWebFlowService webFlowService = new WebFlowService(new ShopaholicContext { });
for (DateTime date = DateTime.Now.Date.AddDays(-29); date <= DateTime.Now.Date; date = date.AddDays(1))
{
    
}