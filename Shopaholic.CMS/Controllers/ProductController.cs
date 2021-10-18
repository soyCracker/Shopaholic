using Microsoft.AspNetCore.Mvc;
using Shopaholic.CMS.Model.Requests;
using Shopaholic.CMS.Model.Response;
using Shopaholic.Entity.Models;
using Shopaholic.Service.Interfaces;


namespace Shopaholic.CMS.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger Logger;
        private readonly IProductService productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            this.Logger = logger;
            this.productService = productService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public MessageModel<AddProductRequest> Add([FromBody]AddProductRequest req)
        {
            Product product = new Product();
            product.Name = req.Name;
            product.Description = req.Description;
            product.CategoryId = req.CategoryId;
            product.Content = req.Content;
            product.Image = req.Image;
            product.Price = req.Price;
            product.Stock = req.Stock;
            bool res = productService.AddProduct(product);
            return new MessageModel<AddProductRequest>
            {
                Success = res,
                Msg = res ? "" : "Add Fail",
                Data = req
            };
        }
    }
}
