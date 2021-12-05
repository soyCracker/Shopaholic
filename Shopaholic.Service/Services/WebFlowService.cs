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

        public List<FlowDTO> GetMonthFlow()
        {
            using (dbContext)
            {
                List<FlowDTO> flowList = new List<FlowDTO>();
                List<WebFlow> flowSource = dbContext.WebFlows.Where(x => x.CreateTime >= DateTime.Now.Date.AddDays(-6) && x.CreateTime < DateTime.Now.Date.AddDays(1) ).ToList();
                for (DateTime date = DateTime.Now.Date.AddDays(-29); date <= DateTime.Now.Date; date = date.AddDays(1))
                {
                    var temp = date.AddDays(1).Date;
                    int flowCount = flowSource.Where(x => x.CreateTime >= date && x.CreateTime < temp).Count();
                    //int noRepeat = dbContext.WebFlows.Where(x => x.CreateTime >= date && x.CreateTime < temp).Select(y => y.Ip).Distinct().Count();
                    FlowDTO model = new FlowDTO()
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
