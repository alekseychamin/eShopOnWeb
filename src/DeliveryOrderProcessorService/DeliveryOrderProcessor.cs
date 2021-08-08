using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DeliveryOrderProcessorService
{
    public static class DeliveryOrderProcessor
    {
        [FunctionName("DeliveryOrderProcessor")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic order = JsonConvert.DeserializeObject(requestBody);

            var endpointUri = Environment.GetEnvironmentVariable("EndpointUri");
            var primaryKey = Environment.GetEnvironmentVariable("PrimaryKey");
            var databaseId = Environment.GetEnvironmentVariable("CosmosDbId");
            var containerId = Environment.GetEnvironmentVariable("CosmosContainerId");
            var partionKeyPath = Environment.GetEnvironmentVariable("CosmosDbPartionKeyPath");

            CosmosClient cosmosClient = new CosmosClient(endpointUri, primaryKey);
            Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);

            Container container = await database.CreateContainerIfNotExistsAsync(containerId, partionKeyPath);
            
            await container.CreateItemAsync(order, new PartitionKey(order.ShipToAddress.ToString()));

            return new OkObjectResult($"Order was saved in CosmosDb with address to deliver: {order.ShipToAddress}");
        }
    }
}
