using Shopaholic.Entity.Models;

namespace Shopaholic.Service.Interfaces
{
    public interface ICategoryService
    {
        List<Category> GetCategoryList();

        Category GetCategory(int id);

        bool AddCategory(string name);

        bool UpdateCategory(int id, string name);

        bool DeleteCategory(int id);
    }
}
