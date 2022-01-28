// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Shopaholic.Entity.Models;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Services;

Console.WriteLine("Hello, World!");


// 寫入測試類別
/*
ICategoryService categoryService = new CategoryService(dbContext);
categoryService.AddCategory("未分類");
int testCategoryId = dbContext.Categories.SingleOrDefault(x => x.Name == "未分類").Id;
*/


// 寫入測試商品
/*
for (int i = 1; i <= 50; i++)
{
    var connectionstring = "Server=.\\SQLEXPRESS;Database=Shopaholic;Trusted_Connection=True;";
    var optionsBuilder = new DbContextOptionsBuilder<ShopaholicContext>();
    optionsBuilder.UseSqlServer(connectionstring);
    ShopaholicContext dbContext = new ShopaholicContext(optionsBuilder.Options);
    IProductService productService = new ProductService(dbContext);
    productService.AddProduct("餅乾測試" + i.ToString().PadLeft(2, '0'), "我的天啊", 1, "我的天啊", null, 9999, 8787);
    Console.WriteLine("Write " + i.ToString().PadLeft(2, '0') + " Success");
}
*/

// 寫入測試flow
/*
IWebFlowService webFlowService = new WebFlowService(dbContext);
for (DateTime date = DateTime.Now.Date.AddDays(-29); date <= DateTime.Now.Date; date = date.AddDays(1))
{
    
}
*/