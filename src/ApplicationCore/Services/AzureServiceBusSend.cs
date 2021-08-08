using Azure.Messaging.ServiceBus;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.ApplicationCore.Services
{
    public class AzureServiceBusSend : IAzureServiceBusSend
    {
        private readonly AzureServiceBusConfig _serviceBusConfig;

        public AzureServiceBusSend(IOptions<AzureServiceBusConfig> serviceBusConfig)
        {
            _serviceBusConfig = serviceBusConfig.Value;
        }
        
        public async Task SendOrder(object order)
        {
            await using var client = new ServiceBusClient(_serviceBusConfig.Connection);

            ServiceBusSender sender = client.CreateSender(_serviceBusConfig.QueueName);

            var json = JsonConvert.SerializeObject(order);
            ServiceBusMessage message = new ServiceBusMessage(json);

            await sender.SendMessageAsync(message);
        }
    }
}
