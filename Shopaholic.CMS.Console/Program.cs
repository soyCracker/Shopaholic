// See https://aka.ms/new-console-template for more information
using Shopaholic.Entity.Models;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Model.Moels;
using Shopaholic.Service.Services;

Console.WriteLine("Hello, World!");


// 寫入測試商品
/*
List<ProductDTO> products = new List<ProductDTO>();
IProductService productService = new ProductService(new ShopaholicContext { });
for (int i = 1; i <= 50; i++)
{
    products.Add(new ProductDTO
    {
        Name = "餅乾測試" + i.ToString().PadLeft(2, '0'),
        Description = "我的天啊",
        CategoryId = 11,
        Content = "我的天啊",
        Image = null,
        Price = 9999,
        Stock = 8787,
        IsDelete = false
    });   
    Console.WriteLine("Write " + i.ToString().PadLeft(2, '0') + " Success");
}
productService.AddProductRange(products);
*/

// 寫入測試flow
/*
Random rnd = new Random();
List<FlowDTO> flowDtoList = new List<FlowDTO>();
IWebFlowService webFlowService = new WebFlowService(new ShopaholicContext { });
for (DateTime date = DateTime.Now.Date.AddDays(-29); date <= DateTime.Now.Date; date = date.AddDays(1))
{
    int rndMax = rnd.Next(5, 50);
    for (int r=0; r<rndMax; r++ )
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
*/