using Azure.Messaging.EventHubs.Consumer;
using CoffeeMachine.EventHub.Model;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoffeeMachine.EventHub.Receiver.Direct
{
    class Program
    {
        private const string CONNECTION_STRING = "";
        static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        private static async Task MainAsync()
        {
            using CancellationTokenSource cancellationSource = new(TimeSpan.FromSeconds(45));

            await using (EventHubConsumerClient eventHubClient = new(EventHubConsumerClient.DefaultConsumerGroupName, CONNECTION_STRING))
            {
                await foreach (PartitionEvent partitionEvent in eventHubClient.ReadEventsAsync(startReadingAtEarliestEvent: false, cancellationToken: cancellationSource.Token))
                {
                    var dataAsJson = Encoding.UTF8.GetString(partitionEvent.Data.EventBody.ToArray());
                    var coffeeMachineData = JsonConvert.DeserializeObject<CoffeeMachineData>(dataAsJson);
                    Console.WriteLine($"{dataAsJson} | PartitionId: { partitionEvent.Partition.PartitionId }");
                }
            };

            Console.ReadKey();
        }
    }
}