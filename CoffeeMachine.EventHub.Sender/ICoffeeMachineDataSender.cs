using CoffeeMachine.EventHub.Sender.Model;
using System.Threading.Tasks;

namespace CoffeeMachine.EventHub.Sender
{
    public interface ICoffeeMachineDataSender
    {
        Task SendDataAsync(CoffeeMachineData data);
    }
}
