using Dapper;
using Microsoft.Data.SqlClient;
using Shopaholic.Entity.Models;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Model.Moels;
using StackExchange.Redis;
using System.Data;
using System.Text.Json;

namespace Shopaholic.Service.Services
{
    public class WebFlowService : IWebFlowService
    {
        private readonly ShopaholicContext dbContext;
        private readonly IDatabase redis;
        private readonly string productFlowTopRedisKey = "ProductFlowTop";
        private readonly string productOrderTopRedisKey = "ProductOrderTop";
        private readonly IDbConnection conn;

        public WebFlowService(ShopaholicContext dbContext, IConnectionMultiplexer connectionMultiplexer, IDbConnection conn)
        {
            this.dbContext = dbContext;
            redis = connectionMultiplexer.GetDatabase();
            this.conn = conn;
        }

        public void AddFlow(string ip, string enter, string email)
        {
            using (dbContext)
            {
                var user = dbContext.CustomerAccounts.SingleOrDefault(x => x.Email == email);
                string userId = user == null ? "" : user.AccountId;
                WebFlow flow = new WebFlow
                {
                    Ip = ip,
                    Enter = enter,
                    UserInfo = userId
                };
                dbContext.WebFlows.Add(flow);
                dbContext.SaveChanges();
            }
        }

        public void AddFlowRange(List<FlowDTO> flowDtoList)
        {
            using (dbContext)
            {
                List<WebFlow> webFlowList = new List<WebFlow>();
                foreach (FlowDTO flowDto in flowDtoList)
                {
                    WebFlow flow = new WebFlow
                    {
                        Ip = flowDto.Ip,
                        Enter = flowDto.Enter,
                        CreateTime = flowDto.CreateTime,
                    };
                    webFlowList.Add(flow);
                }
                dbContext.WebFlows.AddRange(webFlowList);
                dbContext.SaveChanges();
            }
        }

        public List<FlowCountDTO> GetMonthFlow()
        {        
            using (dbContext)
            {
                int flowRange = -29;
                List<DateTime> monthDate = new List<DateTime>();
                for (int i = flowRange; i <= 0; i++)
                {
                    monthDate.Add(DateTime.Now.Date.AddDays(i));
                }

                var flowInRange = dbContext.WebFlows.Where(x => x.CreateTime >= DateTime.Now.Date.AddDays(flowRange) &&
                        x.CreateTime < DateTime.Now.Date.AddDays(1));

                var flowPaddingDate = from m in monthDate
                                      join w in flowInRange on m.Date equals w.CreateTime.Date into sub
                                      from w in sub.DefaultIfEmpty()
                                      select new
                                      {
                                          FlowDate = m.Date,
                                          HasValue = w != null ? w.Id : 0
                                      };

                var flowCountByDate = flowPaddingDate
                    .GroupBy(g => g.FlowDate)
                    .Select(s => new
                    {
                        Count = s.Where(e => e.HasValue != 0).Count(),
                        CreateTime = s.Key
                    }).OrderBy(o => o.CreateTime.Date);

                List<FlowCountDTO> flowList = new List<FlowCountDTO>();
                foreach (var item in flowCountByDate)
                {
                    flowList.Add(new FlowCountDTO
                    {
                        FlowDate = item.CreateTime.ToString("MM/dd"),
                        Count = item.Count
                    });
                }
                return flowList;
            }
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
