﻿using Microsoft.Extensions.Logging;
using Shopaholic.Service.Common.Constants;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Model.Moels;
using Shopaholic.Service.Services;
using Shopaholic.Web.Model.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Base.Console.Class
{
    public class WriteTestDataClass
    {
        private readonly ILogger<WriteTestDataClass> logger;
        private readonly IHttpClientFactory httpClientFactory;

        public WriteTestDataClass(ILogger<WriteTestDataClass> logger, IHttpClientFactory httpClientFactory)
        {
            this.logger = logger;
            this.httpClientFactory = httpClientFactory;
        }

        public void WriteCategory(string conStr)
        {
            ICategoryService categoryService = new CategoryService(DBClass.GetDbContext(conStr));
            categoryService.AddCategory("未分類");
        }

        public void WriteProduct(string conStr)
        {
            List<ProductDTO> products = new List<ProductDTO>();
            IProductService productService = new ProductService(DBClass.GetDbContext(conStr));
            int testCategoryId = DBClass.GetDbContext(conStr).Categories.SingleOrDefault(x => x.Name == "未分類").Id;
            for (int i = 57; i <= 57; i++)
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
                logger.LogDebug("Write " + i.ToString().PadLeft(2, '0') + " Success");
            }
            productService.AddProductRange(products);
        }

        public void WriteFlow(string conStr)
        {
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
                    logger.LogDebug("Write " + r.ToString().PadLeft(2, '0') + " " + date + " Success");
                }
            }
            webFlowService.AddFlowRange(flowDtoList);
        }

        public void WriteAuth(string conStr)
        {
            IAuthService authService = new FirebaseGoogleAuthService(DBClass.GetDbContext(conStr));
            authService.UpdateUser("", "TEST0000001GGG", "TEST_MAN_GGGGGG52869416", "TESTTEST00000123456798@gmail.com.test",
                            true, "", true);
        }

        public void WriteOrder(string conStr)
        {
            List<Task<int>> tasks = new List<Task<int>>();
            TaskFactory factory = new TaskFactory();
            object orderLock = new object();
            tasks.Add(factory.StartNew<int>(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    IPurchaseService purchaseService = new LinePayPurchaseService(DBClass.GetDbContext(conStr), httpClientFactory);
                    List<PurchaseProductModel> purchaseProductList = new List<PurchaseProductModel>();
                    purchaseProductList.Add(new PurchaseProductModel
                    {
                        ProductId = 1,
                        Quantity = 5,
                        CurrentPrice = 777
                    });
                    PurchaseOrderCreateReq req = new PurchaseOrderCreateReq
                    {
                        OrderTypeCode = OrderTypeCode.TEST,
                        UserId = "TEST0000001GGG",
                        ReceiveMan = "TEST_MAN_GGGGGG52869416",
                        Email = "TESTTEST00000123456798@gmail.com.test",
                        Phone = "1000000000",
                        Address = "TEST_ADDRESS",
                        ProductList = purchaseProductList
                    };
                    lock (orderLock)
                        logger.LogDebug("OrderId: " + purchaseService.CreateOrder(req));
                }
                return 1;
            }));


            tasks.Add(factory.StartNew<int>(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    IPurchaseService purchaseService = new LinePayPurchaseService(DBClass.GetDbContext(conStr), httpClientFactory);
                    List<PurchaseProductModel> purchaseProductList = new List<PurchaseProductModel>();
                    purchaseProductList.Add(new PurchaseProductModel
                    {
                        ProductId = 1,
                        Quantity = 5,
                        CurrentPrice = 777
                    });

                    PurchaseOrderCreateReq req = new PurchaseOrderCreateReq
                    {
                        OrderTypeCode = OrderTypeCode.TEST,
                        UserId = "TEST0000001GGG",
                        ReceiveMan = "TEST_MAN_GGGGGG52869416",
                        Email = "TESTTEST00000123456798@gmail.com.test",
                        Phone = "1000000000",
                        Address = "TEST_ADDRESS",
                        ProductList = purchaseProductList
                    };
                    lock (orderLock)
                        logger.LogDebug("OrderId: " + purchaseService.CreateOrder(req));
                }
                return 2;
            }));

            foreach (var t in tasks)
            {
                logger.LogDebug("Task:" + t.Result);
            }
        }
    }
}
