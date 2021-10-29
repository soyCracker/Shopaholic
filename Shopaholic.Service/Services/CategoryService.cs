using Shopaholic.Entity.Models;
using Shopaholic.Service.Interfaces;

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
                    dbContext.Categories.Remove(category);
                    dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public List<Category> GetCategoryList()
        {
            using (dbContext)
            {
                return dbContext.Categories.ToList();
            }
        }

        public Category GetCategory(int id)
        {
            using (dbContext)
            {
                return dbContext.Categories.SingleOrDefault(x => x.Id == id);
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
                    dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
        }
    }
}
