using CoffeeMachine.EventHub.Sender.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoffeeMachine.EventHub.Sender
{
    public interface ICoffeeMachineDataSender
    {
        Task SendDataAsync(CoffeeMachineData data);
        Task SendDataAsync(IEnumerable<CoffeeMachineData> eventData);
    }
}
