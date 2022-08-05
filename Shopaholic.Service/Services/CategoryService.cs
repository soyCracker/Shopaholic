using Shopaholic.Util.Utilities;
using Shopaholic.Entity.Models;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Model.Moels;
using StackExchange.Redis;
using System.Text.Json;

namespace Shopaholic.Service.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ShopaholicContext dbContext;
        private readonly IDatabase redis;
        private readonly string categoryListRedisKey = "CategoryList";

        public CategoryService(ShopaholicContext dbContext, IConnectionMultiplexer connectionMultiplexer)
        {
            this.dbContext = dbContext;
            redis = connectionMultiplexer.GetDatabase();
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
                    //更新資料後清空redis key item
                    redis.KeyDelete(categoryListRedisKey);
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
                    category.UpdateTime = TimeUtil.GetUtcDateTime().UtcDateTime;
                    dbContext.SaveChanges();
                    //更新資料後清空redis key item
                    redis.KeyDelete(categoryListRedisKey);
                    return true;
                }
                return false;
            }
        }

        public List<CategoryDTO> GetCategoryList()
        {
            var redisData = GetCategoryListFromRedis(categoryListRedisKey);
            if (redisData != null)
            {
                return redisData;
            }
            var dbData = GetCategoryListFromDB();
            SaveCategoryListToRedis(categoryListRedisKey, dbData);
            return dbData;
        }

        public CategoryDTO GetCategory(int id)
        {
            using (dbContext)
            {
                Category category = dbContext.Categories.SingleOrDefault(x => x.Id == id && x.IsDelete == false);
                if(category != null)
                {
                    CategoryDTO categoryDTO = new CategoryDTO
                    {
                        Name = category.Name,
                        Id = category.Id
                    };
                    return categoryDTO;
                }
                return null;
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
                    origin.UpdateTime = TimeUtil.GetUtcDateTime().UtcDateTime;
                    dbContext.SaveChanges();
                    //更新資料後清空redis key item
                    redis.KeyDelete(categoryListRedisKey);
                    return true;
                }
                return false;
            }
        }

        public CategoryDTO GetCategory(string name)
        {
            using(dbContext)
            {
                Category category = dbContext.Categories.SingleOrDefault(x => x.Name == name && x.IsDelete == false);
                if(category != null)
                {
                    CategoryDTO categoryDTO = new CategoryDTO
                    {
                        Name = category.Name,
                        Id = category.Id
                    };
                    return categoryDTO;
                }
                return null;
            }
        }

        private List<CategoryDTO> GetCategoryListFromDB()
        {
            using (dbContext)
            {
                List<Category> categoryList = dbContext.Categories.Where(x => x.IsDelete == false).ToList();
                List<CategoryDTO> categoryDTOList = new List<CategoryDTO>();
                foreach (Category category in categoryList)
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

        private void SaveCategoryListToRedis(string redisKey, List<CategoryDTO> categoryDTOs)
        {
            string str = JsonSerializer.Serialize(categoryDTOs);
            TimeSpan expire = TimeSpan.FromMinutes(5);
            redis.StringSet(redisKey, str, expire);
        }

        private List<CategoryDTO> GetCategoryListFromRedis(string redisKey)
        {
            if (redis.KeyExists(redisKey))
            {
                string str = redis.StringGet(redisKey);
                return JsonSerializer.Deserialize<List<CategoryDTO>>(str);
            }
            return null;
        }
    }
}
