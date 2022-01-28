using Microsoft.EntityFrameworkCore;
using Shopaholic.Entity.Models;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Model.Moels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
                int flowRange = -29;                
                var webFlowCountByDate = dbContext.WebFlows.Where(x => x.CreateTime >= DateTime.Now.Date.AddDays(flowRange) && x.CreateTime < DateTime.Now.Date.AddDays(1))
                    .GroupBy(g => g.CreateTime.Date)
                    .Select(s=>new
                    {
                        Count = s.Count(),
                        CreateTime = s.Key
                    }).OrderBy(o=>o.CreateTime.Date).AsNoTracking().ToList();
                List<FlowCountDTO> flowList = new List<FlowCountDTO>();
                foreach (var flowDateObj in webFlowCountByDate)
                {
                    flowList.Add(new FlowCountDTO
                    {
                        FlowDate = flowDateObj.CreateTime.ToString("MM/dd"),
                        Count = flowDateObj.Count
                    });
                }
                return flowList;
            }
        }
    }
}
