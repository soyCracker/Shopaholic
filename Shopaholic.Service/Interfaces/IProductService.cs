using Shopaholic.Entity.Models;

namespace Shopaholic.Service.Interfaces
{
    public interface IProductService
    {
        List<Product> GetProductList();

        Product GetProduct(int id);

        bool AddProduct(Product product);  

        bool UpdateProduct(Product product);

        bool DeleteProduct(int id);
    }
}
