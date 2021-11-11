using Microsoft.AspNetCore.Http;
using Shopaholic.Entity.Models;

namespace Shopaholic.Service.Interfaces
{
    public interface IProductService
    {
        List<Product> GetProductList();

        Product GetProduct(int id);

        bool AddProduct(string name, string description, int categoryId, string content, string image, int price, int stock);  

        bool UpdateProduct(int id, string name, string description, int categoryId, string content, string image, int price, 
            int stock);

        bool DeleteProduct(int id);

        List<Product> SearchProduct(string name, string description, string content);
    }
}
