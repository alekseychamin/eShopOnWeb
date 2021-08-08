using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace OrderItemsReserver
{
    public static class OrderItemsReserver
    {
        [FunctionName("OrderItemsReserver")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic order = JsonConvert.DeserializeObject(requestBody);

            var connectionString = Environment.GetEnvironmentVariable("StorageConnectionString");
            var containerName = Environment.GetEnvironmentVariable("ContainerName");

            BlobContainerClient container = new BlobContainerClient(connectionString, containerName);
            await container.CreateIfNotExistsAsync();

            var blobName = $"order {order.id}";
            BlobClient blob = container.GetBlobClient(blobName);

            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(requestBody)))
            {
                await blob.UploadAsync(ms);
            }

            return new OkObjectResult($"Order was saved in blobstorage with name: {blobName}");
        }
    }
}
