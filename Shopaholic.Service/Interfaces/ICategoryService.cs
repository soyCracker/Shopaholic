using Shopaholic.Service.Model.Moels;

namespace Shopaholic.Service.Interfaces
{
    public interface ICategoryService
    {
        List<CategoryDTO> GetCategoryList();

        CategoryDTO GetCategory(int id);

        bool AddCategory(string name);

        bool UpdateCategory(int id, string name);

        bool DeleteCategory(int id);

        CategoryDTO GetCategory(string name);
    }
}
