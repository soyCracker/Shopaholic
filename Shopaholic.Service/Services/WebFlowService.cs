using Shopaholic.Entity.Models;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Model.Moels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Service.Services
{
    public class WebFlowService : IWebFlowService
    {
        private readonly ShopaholicContext dbContext;

        public WebFlowService(ShopaholicContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void AddFlow(string ip, string enter)
        {
            using(dbContext)
            {
                WebFlow flow = new WebFlow
                {
                    Ip = ip,
                    Enter = enter
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
                List<FlowCountDTO> flowList = new List<FlowCountDTO>();
                List<WebFlow> flowSource = dbContext.WebFlows.Where(x => x.CreateTime >= DateTime.Now.Date.AddDays(-6) && x.CreateTime < DateTime.Now.Date.AddDays(1) ).ToList();
                for (DateTime date = DateTime.Now.Date.AddDays(-29); date <= DateTime.Now.Date; date = date.AddDays(1))
                {
                    var temp = date.AddDays(1).Date;
                    int flowCount = flowSource.Where(x => x.CreateTime >= date && x.CreateTime < temp).Count();
                    //int noRepeat = dbContext.WebFlows.Where(x => x.CreateTime >= date && x.CreateTime < temp).Select(y => y.Ip).Distinct().Count();
                    FlowCountDTO model = new FlowCountDTO()
                    {
                        FlowDate = date.ToString("MM/dd"),
                        Count = flowCount
                    };
                    flowList.Add(model);
                }
                return flowList;
            }
        }
    }
}
