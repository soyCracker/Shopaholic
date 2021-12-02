using Microsoft.AspNetCore.Http;
using Shopaholic.Entity.Models;
using Shopaholic.Service.Model.Moels;

namespace Shopaholic.Service.Interfaces
{
    public interface IProductService
    {
        List<ProductDTO> GetProductList();

        ProductDTO GetProduct(int id);

        bool AddProduct(string name, string description, int categoryId, string content, string image, int price, int stock);  

        bool UpdateProduct(int id, string name, string description, int categoryId, string content, string image, int price, 
            int stock);

        bool DeleteProduct(int id);

        List<ProductDTO> SearchProduct(string name, string description, string content);

        ProductWithAllCategoryDTO GetProductWithAllCategory(int id);
    }
}
