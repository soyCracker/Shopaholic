using Shopaholic.Entity.Models;
using Shopaholic.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Service.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ShopaholicContext dbContext;

        public CategoryService(ShopaholicContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool AddCategory(Category category)
        {
            using (dbContext)
            {
                Category exist = dbContext.Categories.SingleOrDefault(x => x.Name == category.Name);
                if (exist == null)
                {
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

        public bool UpdateCategory(Category category)
        {
            using (dbContext)
            {
                Category origin = dbContext.Categories.SingleOrDefault(x => x.Id == category.Id);
                if (origin != null)
                {
                    origin.Name = category.Name;
                    dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
        }
    }
}
