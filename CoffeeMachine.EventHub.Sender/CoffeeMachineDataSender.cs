using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using CoffeeMachine.EventHub.Sender.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMachine.EventHub.Sender
{
    public class CoffeeMachineDataSender : ICoffeeMachineDataSender
    {
        private EventHubProducerClient _eventHubClient;

        public CoffeeMachineDataSender(string eventHubConnectionString)
        {
            _eventHubClient = new EventHubProducerClient(eventHubConnectionString);
        }

        public async Task SendDataAsync(CoffeeMachineData data)
        {
            var JSONData = JsonConvert.SerializeObject(data);
            using EventDataBatch eventBatch = await _eventHubClient.CreateBatchAsync();
            eventBatch.TryAdd(new EventData(JSONData));

            await _eventHubClient.SendAsync(eventBatch);
        }

        public async Task SendDataAsync(IEnumerable<CoffeeMachineData> eventData)
        {
            var eventBatch = await _eventHubClient.CreateBatchAsync();
            _ = eventData.Select(item => eventBatch.TryAdd(CreateEventData(item)));
            
            await _eventHubClient.SendAsync(eventBatch);
        }

        private EventData CreateEventData(CoffeeMachineData eventData)
        {
            return new EventData(
                        Encoding.UTF8.GetBytes(
                            JsonConvert.SerializeObject(eventData)
                        )
                       );
        }
    }
}