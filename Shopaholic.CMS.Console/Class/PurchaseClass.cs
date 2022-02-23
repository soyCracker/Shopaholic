using Shopaholic.Service.Common.Constants;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Services;
using Shopaholic.Web.Model.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.CMS.Console.Class
{
    public class PurchaseClass
    {
        public static void Create(string conStr)
        {
            for (int i = 0; i < 10; i++)
            {
                IPurchaseService purchaseService = new TestPurchaseService(DBClass.GetDbContext(conStr));
                List<PurchaseProductModel> purchaseProductList = new List<PurchaseProductModel>();
                purchaseProductList.Add(new PurchaseProductModel
                {
                    ProductId = 1,
                    Quantity = 5,
                    CurrentPrice = 777
                });
                PurchaseReq req = new PurchaseReq
                {
                    OrderTypeCode = OrderTypeCode.TEST,
                    UserId = "TEST0000001GGG",
                    ReceiveMan = "TEST_MAN_GGGGGG52869416",
                    Email = "TESTTEST00000123456798@gmail.com.test",
                    Phone = "1000000000",
                    Address = "TEST_ADDRESS",
                    ProductList = purchaseProductList
                };
            }
        }
    }
}
