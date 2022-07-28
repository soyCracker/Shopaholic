using Shopaholic.Service.Model.Moels;

namespace Shopaholic.Service.Interfaces
{
    public interface IWebFlowService
    {
        List<FlowCountDTO> GetMonthFlow();

        void AddFlow(string ip, string enter, string userId);

        void AddFlowRange(List<FlowDTO> flowDtoList);
    }
}
