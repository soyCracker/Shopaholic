using Shopaholic.Util.Utilities;
using Shopaholic.Entity.Models;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Model.Moels;

namespace Shopaholic.Service.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ShopaholicContext dbContext;

        public CategoryService(ShopaholicContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool AddCategory(string name)
        {
            using (dbContext)
            {
                Category exist = dbContext.Categories.SingleOrDefault(x => x.Name == name);
                if (exist == null)
                {
                    Category category = new Category
                    {
                        Name = name,
                    };
                    dbContext.Categories.Add(category);
                    dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public bool DeleteCategory(int id)
        {
            using (dbContext)
            {
                Category category = dbContext.Categories.SingleOrDefault(x => x.Id == id);
                if (category != null)
                {
                    category.IsDelete = true;
                    category.UpdateTime = TimeUtil.GetLocalDateTime();
                    dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public List<CategoryDTO> GetCategoryList()
        {
            using (dbContext)
            {
                List<Category> categoryList = dbContext.Categories.Where(x => x.IsDelete == false).ToList();
                List<CategoryDTO> categoryDTOList = new List<CategoryDTO>();
                foreach(Category category in categoryList)
                {
                    CategoryDTO categoryDTO = new CategoryDTO
                    {
                        Name = category.Name,
                        Id = category.Id
                    };
                    categoryDTOList.Add(categoryDTO);
                }
                return categoryDTOList;
            }
        }

        public CategoryDTO GetCategory(int id)
        {
            using (dbContext)
            {
                Category category = dbContext.Categories.SingleOrDefault(x => x.Id == id && x.IsDelete == false);
                CategoryDTO categoryDTO = new CategoryDTO
                {
                    Name = category.Name,
                    Id = category.Id
                };
                return categoryDTO;
            }
        }

        public bool UpdateCategory(int id, string name)
        {
            using (dbContext)
            {
                Category origin = dbContext.Categories.SingleOrDefault(x => x.Id == id);
                if (origin != null)
                {
                    origin.Name = name;
                    origin.UpdateTime = TimeUtil.GetLocalDateTime();
                    dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
        }
    }
}
