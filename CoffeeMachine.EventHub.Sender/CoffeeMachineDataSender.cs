using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using CoffeeMachine.EventHub.Sender.Model;
using Newtonsoft.Json;
using System;
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
    }
}