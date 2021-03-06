using Shopaholic.Service.Model.Moels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Service.Interfaces
{
    public interface IWebFlowService
    {
        List<FlowCountDTO> GetMonthFlow();

        void AddFlow(string ip, string enter, string userId);

        void AddFlowRange(List<FlowDTO> flowDtoList);

        List<ProductTopDTO> GetProductByMonthFlowTop();

        List<ProductTopDTO> GetProductByMonthOrderTop();

    }
}
