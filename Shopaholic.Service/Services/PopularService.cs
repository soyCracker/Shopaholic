using Shopaholic.Entity.Models;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Model.Moels;
using StackExchange.Redis;
using System.Data;
using System.Text.Json;

namespace Shopaholic.Service.Services
{
    public class PopularService : IPopularService
    {
        private readonly ShopaholicContext dbContext;
        private readonly IDatabase redis;
        private readonly string productFlowTopRedisKey = "ProductFlowTop";
        private readonly string productOrderTopRedisKey = "ProductOrderTop";

        public PopularService(ShopaholicContext dbContext, IConnectionMultiplexer connectionMultiplexer)
        {
            this.dbContext = dbContext;
            redis = connectionMultiplexer.GetDatabase();
        }

        public List<ProductTopDTO> GetProductByMonthFlowTop()
        {
            var redisData = GetTopFlowFromRedis(productFlowTopRedisKey);
            if (redisData != null)
            {
                return redisData;
            }
            var dbData = GetProductByMonthFlowTopFromDB();
            SaveTopFlowToRedis(productFlowTopRedisKey, dbData);
            return dbData;
        }

        public List<ProductTopDTO> GetProductByMonthOrderTop()
        {
            var redisData = GetTopFlowFromRedis(productOrderTopRedisKey);
            if (redisData != null)
            {
                return redisData;
            }
            var dbData = GetProductByMonthOrderTopFromDB();
            SaveTopFlowToRedis(productOrderTopRedisKey, dbData);
            return dbData;
        }

        private List<ProductTopDTO> GetProductByMonthFlowTopFromDB()
        {
            using (dbContext)
            {
                int flowRange = -29;
                var flowInRange = dbContext.WebFlows.Where(x => x.CreateTime >= DateTime.Now.Date.AddDays(flowRange)
                    && x.CreateTime < DateTime.Now.Date.AddDays(1)).AsEnumerable();
                var group = flowInRange.GroupBy(x => x.Enter)
                    .Select(s => new
                    {
                        ProductId = s.Key.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Last(),
                        Count = s.Count(),
                        Enter = s.Key
                    });
                var topFive = group.OrderByDescending(o => o.Count).Take(5);
                List<ProductTopDTO> productTopDTOs = new List<ProductTopDTO>();
                foreach (var flow in topFive)
                {
                    var product = dbContext.Products.SingleOrDefault(x => x.Id.ToString() == flow.ProductId);
                    if (product != null)
                    {
                        productTopDTOs.Add(new ProductTopDTO
                        {
                            Id = product.Id,
                            Name = product.Name,
                            Price = product.Price,
                            Image = product.Image,
                            Count = flow.Count,
                        });
                    }
                }
                return productTopDTOs;
            }
        }

        private List<ProductTopDTO> GetProductByMonthOrderTopFromDB()
        {
            using (dbContext)
            {
                int dateRange = -29;
                var orderInRange = dbContext.OrderDetails.Where(x => x.CreateTime >= DateTime.Now.Date.AddDays(dateRange)
                    && x.CreateTime < DateTime.Now.Date.AddDays(1)).AsEnumerable();
                var group = orderInRange.GroupBy(x => x.ProductId)
                    .Select(s => new
                    {
                        ProductId = s.Key,
                        Count = s.Count(),
                    });
                var topFive = group.OrderByDescending(o => o.Count).Take(5);
                List<ProductTopDTO> productTopDTOs = new List<ProductTopDTO>();
                foreach (var order in topFive)
                {
                    var product = dbContext.Products.SingleOrDefault(x => x.Id == order.ProductId);
                    if (product != null)
                    {
                        productTopDTOs.Add(new ProductTopDTO
                        {
                            Id = product.Id,
                            Name = product.Name,
                            Price = product.Price,
                            Image = product.Image,
                            Count = order.Count,
                        });
                    }
                }
                return productTopDTOs;
            }
        }

        private void SaveTopFlowToRedis(string redisKey, List<ProductTopDTO> productTopDTOs)
        {
            string str = JsonSerializer.Serialize(productTopDTOs);
            TimeSpan expire = TimeSpan.FromMinutes(5);
            redis.StringSet(redisKey, str, expire);
        }

        private List<ProductTopDTO> GetTopFlowFromRedis(string redisKey)
        {
            if (redis.KeyExists(redisKey))
            {
                string str = redis.StringGet(redisKey);
                return JsonSerializer.Deserialize<List<ProductTopDTO>>(str);
            }
            return null;
        }
    }
}
