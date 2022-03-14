using Microsoft.AspNetCore.Http;
using Shopaholic.Entity.Models;
using Shopaholic.Service.Model.Moels;

namespace Shopaholic.Service.Interfaces
{
    public interface IProductService
    {
        ProductDTO GetProduct(int id);

        bool AddProduct(string name, string description, int categoryId, string content, string image, int price, int stock);

        void AddProductRange(List<ProductDTO> productDTOs);

        bool UpdateProduct(int id, string name, string description, int categoryId, string content, string image, int price, 
            int stock);

        bool DeleteProduct(int id);

        ProductSearchResDTO AdminSearch(string searchStr, int page, int pageSize);

        ProductSearchResDTO Search(int? categoryId, string searchStr, int page, int pageSize);

        ProductWithAllCategoryDTO GetProductWithAllCategory(int id);

        List<ProductTopDTO> GetProductByMonthFlowTop();

        List<ProductTopDTO> GetProductByMonthOrderTop();
    }
}
