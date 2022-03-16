using Microsoft.Extensions.Logging;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Model.Moels;
using Shopaholic.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Base.Console.Class
{
    public class WriteFormalDataClass
    {
        private readonly ILogger<WriteFormalDataClass> logger;

        public WriteFormalDataClass(ILogger<WriteFormalDataClass> logger)
        {
            this.logger = logger;
        }

        public void WriteCategory(string conStr)
        {
            ICategoryService categoryService1 = new CategoryService(DBClass.GetDbContext(conStr));
            categoryService1.AddCategory("食品");
            ICategoryService categoryService2 = new CategoryService(DBClass.GetDbContext(conStr));
            categoryService2.AddCategory("通訊");
            ICategoryService categoryService3 = new CategoryService(DBClass.GetDbContext(conStr));
            categoryService3.AddCategory("遊戲");
            ICategoryService categoryService4 = new CategoryService(DBClass.GetDbContext(conStr));
            categoryService4.AddCategory("裝飾");
            ICategoryService categoryService5 = new CategoryService(DBClass.GetDbContext(conStr));
            categoryService5.AddCategory("日用");
        }

        public void WriteFoodProduct(string conStr)
        {
            List<ProductDTO> products = new List<ProductDTO>();
            IProductService productService = new ProductService(DBClass.GetDbContext(conStr));
            int testCategoryId = DBClass.GetDbContext(conStr).Categories.SingleOrDefault(x => x.Name == "食品").Id;
            products.Add(new ProductDTO
            {
                Name = "雀巢蜂蜜脆片早餐脆片",
                Description = "★添加十種重要的維生素與礦物質\n★加杯牛奶，滿足每日的鈣質需求量\n★全家人早餐的最佳選擇\n",
                CategoryId = testCategoryId,
                Content = @"<p><img src=""https://i.imgur.com/7tm5x1j.png"" style=""width: 1000px;""></p><p><img src=""https://i.imgur.com/NykW5gC.jpeg"" style=""width: 1000px;""><br></p>",
                Image = "https://cs-e.ecimg.tw/items/DBAK0OA82843623/000001_1628671133.jpg",
                Price = 132,
                Stock = 73,
                IsDelete = false
            });
            products.Add(new ProductDTO
            {
                Name = "義美 小泡芙巧克力171G量販包(3入/盒)",
                Description = "◆ 最暢銷最長青的義美食品泡芙系列 \n◆ 香鬆脆脆的外殼與柔滑香濃的巧克力內餡 \n◆ 一顆一口方便食用，愈吃愈ㄙㄨㄚˋ嘴，適合各年齡層食用。",
                CategoryId = testCategoryId,
                Content = @"<img src=""https://cs-c.ecimg.tw/items/DBACCRA75927477/i010002_1547623595.jpg"" alt="""" style=""display: inline; border: 0px; color: rgb(102, 102, 102); font-family: Arial, Verdana, &quot;Microsoft JhengHei&quot;, Helvetica, sans-serif; font-size: 13px; text-align: center;""><div class=""tl"" style=""margin: 0px; padding: 0px; font-size: 13px; color: rgb(102, 102, 102); font-family: Arial, Verdana, &quot;Microsoft JhengHei&quot;, Helvetica, sans-serif;""><p style=""margin: 12px 0px; padding: 0px; line-height: 1.6;""><span style=""margin: 0px; padding: 0px; font-size: medium; color: rgb(201, 0, 38);""><span style=""font-family: Arial, Verdana, &quot;microsoft jhenghei&quot;, Helvetica, sans-serif; font-weight: 700;"">★商品規格<br></span><span style=""margin: 0px; padding: 0px; font-size: 16px; color: rgb(0, 0, 0);"">171公克</span><br><br></span></p><p style=""margin: 12px 0px; padding: 0px; line-height: 1.6;""><span style=""margin: 0px; padding: 0px; font-size: medium;""><span style=""font-family: Arial, Verdana, &quot;microsoft jhenghei&quot;, Helvetica, sans-serif; font-weight: 700;""><span style=""margin: 0px; padding: 0px; font-size: 16px; color: rgb(201, 0, 38);"">★商品成分</span></span></span><br><span style=""margin: 0px; padding: 0px; font-size: medium; color: rgb(0, 0, 0);"">植物油(棕櫚油、菜籽油、乳化劑（脂肪酸甘油酯、脂肪酸丙二醇酯）、抗氧化劑（混合濃縮生育醇））、雞蛋、蔗糖、麵粉、乳清粉、可可膏、奶粉、大豆卵磷脂、香料</span></p><p style=""margin: 12px 0px; padding: 0px; line-height: 1.6;""><span style=""margin: 0px; padding: 0px; font-size: medium;"">&nbsp;</span></p><p style=""margin: 12px 0px; padding: 0px; line-height: 1.6;""><span style=""margin: 0px; padding: 0px; font-size: medium;""><span style=""margin: 0px; padding: 0px; font-size: 16px; color: rgb(201, 0, 38);""><span style=""font-family: Arial, Verdana, &quot;microsoft jhenghei&quot;, Helvetica, sans-serif; font-weight: 700;"">★營養標示<br></span></span><span style=""margin: 0px; padding: 0px; font-size: 16px; color: rgb(0, 0, 0);""><br>每一份量&nbsp;&nbsp; 57&nbsp; 公克<br>本包裝含&nbsp;&nbsp;&nbsp;&nbsp; 3&nbsp;&nbsp; 份<br>每份<br>熱量&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;331&nbsp;&nbsp;大卡<br>蛋白質&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; 6.0&nbsp; 公克<br>脂肪&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 22.1&nbsp; 公克<br>&nbsp;飽和脂肪&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; 11.7&nbsp; 公克<br>&nbsp;反式脂肪&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp;0.0&nbsp; 公克<br>碳水化合物&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;27.0&nbsp; 公克<br>糖&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;10.7 &nbsp;公克<br>鈉&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;71&nbsp;&nbsp; 毫克<br><br><span style=""margin: 0px; padding: 0px; color: red; font-family: 新細明體;"">※</span><span style=""font-family: Arial, Verdana, &quot;microsoft jhenghei&quot;, Helvetica, sans-serif; font-weight: 700;""><span style=""margin: 0px; padding: 0px; color: red; font-family: 新細明體;"">營養標示數據若與包裝上略有差異時，以實際包裝上標示為準</span></span><br><br></span><span style=""margin: 0px; padding: 0px; font-size: 16px; color: rgb(201, 0, 38);""><span style=""font-family: Arial, Verdana, &quot;microsoft jhenghei&quot;, Helvetica, sans-serif; font-weight: 700;"">★保存期限<br></span><span style=""margin: 0px; padding: 0px; color: rgb(0, 0, 0);"">一年</span></span></span></p></div>",
                Image = "https://cs-c.ecimg.tw/items/DBACCRA75927477/000001_1620878123.jpg",
                Price = 86,
                Stock = 100,
                IsDelete = false
            });
            productService.AddProductRange(products);
        }

        public void WritePhoneProduct(string conStr)
        {
            List<ProductDTO> products = new List<ProductDTO>();
            IProductService productService = new ProductService(DBClass.GetDbContext(conStr));
            int testCategoryId = DBClass.GetDbContext(conStr).Categories.SingleOrDefault(x => x.Name == "通訊").Id;
            products.Add(new ProductDTO
            {
                Name = "Apple iPhone 13 Pro Max (1TB)-松嶺青色(MND23TA/A)",
                Description = @"• 6.7 吋超 Retina XDR 顯示器，具備 ProMotion 自動適應更新頻率技術，帶來更快速、更靈敏的體驗
                                • 電影級模式為影片增添淺景深效果，並自動變換焦點
                                • 專業級相機系統，配備全新 1200 萬像素望遠、廣角與超廣角相機；光學雷達掃描儀；6 倍光學變焦範圍；微距攝影；具備攝影風格、ProRes 影片4、智慧型 HDR 4、夜間模式、Apple ProRAW、4K 杜比視界 HDR 錄製
                                • 1200萬像素原深感測前置相機
                                • 具備夜間模式、4K 杜比視界 HDR 錄製
                                • A15 仿生晶片，效能表現快如閃電
                                • 影片播放最長可達28小時，iPhone歷來最持久的電池續航力
                                • 超瓷晶盾，設計經久耐用
                                • 領先業界的 IP68 等級防潑抗水功能
                                • 5G 帶來超快速下載與高品質串流播放
                                • iOS 15滿載全新功能而來，讓你的iPhone能比以往做到更多
                                • 支援 MagSafe 配件，可輕鬆貼合，無線充電更快捷7
                                • NCC許可字號：CCAI215G0110T7",
                CategoryId = testCategoryId,
                Content = @"<div class=""tl"" style=""margin: 0px; padding: 0px; font-size: 13px; color: rgb(102, 102, 102); font-family: Arial, Verdana, &quot; Microsoft JhengHei&quot;, Helvetica, sans-serif; ""><p style=""margin: 12px 0px; padding: 0px; line-height: 1.6; ""><img src=""https://i.imgur.com/5sFwy9k.jpeg"" style=""width: 395px;""><br></p></div>",
                Image = "https://cs-b.ecimg.tw/items/DYAJC2A900EO4SV/000001_1646877490.jpg",
                Price = 54400,
                Stock = 13,
                IsDelete = false
            });
            products.Add(new ProductDTO
            {
                Name = "realme GT(8+128) 深海飛艇",
                Description = @"■ Qualcomm® Snapdragon™ 888 旗艦 5G 處理器
                                ■ 120Hz Super AMOLED 電競螢幕
                                ■ 65W 超級閃充/35分鐘充至 100%
                                ■ 3D 鋼化水冷散熱/核心溫度最高降低 15℃
                                ■ 超薄 8.4mm/超輕 186g
                                ■ 滿血版 LPDDR5 + UFS 3.1/超高讀寫速度
                                ■ 杜比全景聲雙喇叭/Dolby Atmos＆Hi-Res 雙認證
                                ■ GT 模式/一鍵釋放極限性能
                                ■ 支援Line全系列完整服務(含Line Pay)",
                CategoryId = testCategoryId,
                Content = @"<p><img src=""https://i.imgur.com/2kDCAzn.png"" style=""width: 1040px;""></p>",
                Image = "https://cs-c.ecimg.tw/items/DYAA5FA900BD59M/000001_1646112572.jpg",
                Price = 12499,
                Stock = 35,
                IsDelete = false
            });
            productService.AddProductRange(products);
        }

        public void WriteGameProduct(string conStr)
        {
            List<ProductDTO> products = new List<ProductDTO>();
            IProductService productService = new ProductService(DBClass.GetDbContext(conStr));
            int testCategoryId = DBClass.GetDbContext(conStr).Categories.SingleOrDefault(x => x.Name == "遊戲").Id;
            products.Add(new ProductDTO
            {
                Name = "【SONY 索尼】PlayStation5 光碟版主機(CFI-1118A01)",
                Description = @"■ 高速SSD 疾如閃電的載入速度
                                ■ 新一代光線追蹤功能
                                ■ 整合式I/O 系統客製化設計",
                CategoryId = testCategoryId,
                Content = @"<p><img src=""https://i.imgur.com/rLUxetG.png"" style=""width: 1000px; ""><br></p>",
                Image = "https://store.sony.com.tw/resource/file/product_files/CFI-1118A01/14_a05c55101_001055502.jpg",
                Price = 15980,
                Stock = 77,
                IsDelete = false
            });
            products.Add(new ProductDTO
            {
                Name = "【Microsoft 微軟】Xbox Series S 512GB遊戲主機_無光碟版 加贈Game Pass 終極版3個月(實體卡)",
                Description = @"■ 功能和速度的絕佳平衡
                                ■ HDR 高動態範圍效果
                                ■ 120FPS 支援",
                CategoryId = testCategoryId,
                Content = @"<p><img src=""https://i.imgur.com/heb5NVI.png"" style=""width: 1000px;""><br></p>",
                Image = "https://compass-ssl.xbox.com/assets/b7/41/b7414f03-9878-4ed3-a9a4-b4ab8f19ca97.jpg?n=0202999-Hero-M.jpg",
                Price = 9980,
                Stock = 87,
                IsDelete = false
            });
            products.Add(new ProductDTO
            {
                Name = "【Nintendo 任天堂】Switch動物森友會主機+《寶可夢傳說 阿爾宙斯-附首批特典及MOMO特典》附《保護貼》",
                Description = @"■ 動物森友會造型主機
                                ■ 附""寶可夢傳說 阿爾宙斯""
                                ■ 台灣公司貨 一年保固",
                CategoryId = testCategoryId,
                Content = @"<p><img src=""https://i.imgur.com/fwVHcUb.png"" style=""width: 1000px;""><br></p>",
                Image = "https://i2.momoshop.com.tw/1645066452/goodsimg/0009/839/887/9839887_R.webp",
                Price = 10450,
                Stock = 131,
                IsDelete = false
            });
            productService.AddProductRange(products);
        }

        public void WriteDecorateProduct(string conStr)
        {
            List<ProductDTO> products = new List<ProductDTO>();
            IProductService productService = new ProductService(DBClass.GetDbContext(conStr));
            int testCategoryId = DBClass.GetDbContext(conStr).Categories.SingleOrDefault(x => x.Name == "裝飾").Id;
            products.Add(new ProductDTO
            {
                Name = "春日清新乾燥花玻璃盅",
                Description = @"■ 觀賞用 增添居家氛圍 送禮自用皆宜
                                ■ 商品尺寸:長11X寬11X高17公分
                                ■ 商品重量:0.5公斤",
                CategoryId = testCategoryId,
                Content = @"<p style=""margin-right: 0px; margin-bottom: 0px; margin-left: 0px; font-family: &quot;Noto Sans TC&quot;, HelveticaNeue, PingFangTC, &quot;Microsoft JhengHei&quot;, &quot;sans-serif&quot;; padding: 0px; border: 0px; outline: 0px; vertical-align: baseline; background-color: rgb(236, 236, 236);""><img alt="""" src=""https://pcm.trplus.com.tw/sys-master/productImages/h49/h1d/10098235375646/000000000014300223-desc-20210427205022154-0.jpg"" style=""vertical-align: baseline; margin: 0px; padding: 0px; border: 0px; outline: 0px; font-weight: inherit; font-style: inherit; font-family: inherit; width: 1000px; height: 1201px;""></p><br style=""font-family: &quot;Noto Sans TC&quot;, HelveticaNeue, PingFangTC, &quot;Microsoft JhengHei&quot;, &quot;sans-serif&quot;; background-color: rgb(236, 236, 236);""><p style=""margin-right: 0px; margin-bottom: 0px; margin-left: 0px; font-family: &quot;Noto Sans TC&quot;, HelveticaNeue, PingFangTC, &quot;Microsoft JhengHei&quot;, &quot;sans-serif&quot;; padding: 0px; border: 0px; outline: 0px; vertical-align: baseline; background-color: rgb(236, 236, 236);""><span style=""margin: 0px; padding: 0px; border: 0px; outline: 0px; font-weight: inherit; font-style: inherit; font-size: 18px; font-family: inherit; vertical-align: baseline;""><span style=""margin: 0px; padding: 0px; border: 0px; outline: 0px; font-weight: inherit; font-style: inherit; font-family: inherit; vertical-align: baseline; color: rgb(255, 0, 0);"">-注意事項-</span></span></p><br style=""font-family: &quot;Noto Sans TC&quot;, HelveticaNeue, PingFangTC, &quot;Microsoft JhengHei&quot;, &quot;sans-serif&quot;; background-color: rgb(236, 236, 236);""><p style=""margin-right: 0px; margin-bottom: 0px; margin-left: 0px; font-family: &quot;Noto Sans TC&quot;, HelveticaNeue, PingFangTC, &quot;Microsoft JhengHei&quot;, &quot;sans-serif&quot;; padding: 0px; border: 0px; outline: 0px; vertical-align: baseline; background-color: rgb(236, 236, 236);"">● 網頁產品因拍攝或螢幕設定不同,可能與實品略有差異，請以實際商品為準。<br>● 圖檔為情境示意圖,本商品內容不包含擺飾物品。<br>● 7天鑑賞期非試用期,商品一經使用或組裝後,恕無法辦理退換貨。</p>",
                Image = "https://pcm.trplus.com.tw/650x650/sys-master/productImages/hc7/h24/10098235113502/000000000014300223-gallery-03-20210427205022154.jpg",
                Price = 1280,
                Stock = 20,
                IsDelete = false
            });
            productService.AddProductRange(products);
        }

        public void WriteDailyUseProduct(string conStr)
        {
            List<ProductDTO> products = new List<ProductDTO>();
            IProductService productService = new ProductService(DBClass.GetDbContext(conStr));
            int testCategoryId = DBClass.GetDbContext(conStr).Categories.SingleOrDefault(x => x.Name == "日用").Id;
            products.Add(new ProductDTO
            {
                Name = "五月花 清膚柔潤抽取衛生紙(100抽x24包x3串/箱)",
                Description = @"◆採用茶樹抗菌因子+乳木果油精粹
                                ◆清膚柔潤 無香味 更安心
                                ◆給您更加柔軟不易破、舒適又好用的衛生紙新體驗！
                                ◆本衛生紙可安心丟入馬桶 (迅速分散於水中)",
                CategoryId = testCategoryId,
                Content = @"<p><img src=""https://i.imgur.com/qzuRPJG.png"" style=""width: 750px;""><br></p>",
                Image = "https://c.ecimg.tw/items/DAAG4EA9008SBD8/i010010_1604022501.jpg",
                Price = 899,
                Stock = 38,
                IsDelete = false
            });
            productService.AddProductRange(products);
        }
    }
}
