using Shopaholic.Entity.Models;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Model.Moels;
using System.Data;

namespace Shopaholic.Service.Services
{
    public class WebFlowService : IWebFlowService
    {
        private readonly ShopaholicContext dbContext;

        public WebFlowService(ShopaholicContext dbContext)
        {
            this.dbContext = dbContext;
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
    }
}
