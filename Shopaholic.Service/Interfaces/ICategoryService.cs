using Shopaholic.Entity.Models;

namespace Shopaholic.Service.Interfaces
{
    public interface ICategoryService
    {
        List<Category> GetCategoriesList();

        Category GetCategory(int id);

        bool AddCategory(Category category);

        bool UpdateCategory(Category category);

        bool DeleteCategory(int id);
    }
}
