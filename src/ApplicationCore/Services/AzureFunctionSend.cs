using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Microsoft.eShopWeb.ApplicationCore.Services
{
    public class AzureFunctionSend : IAzureFunctionSend
    {
        private readonly AzureFunctionConfig _functionConfig;

        public AzureFunctionSend(IOptions<AzureFunctionConfig> functionConfig)
        {
            _functionConfig = functionConfig.Value;
        }
        
        public async Task SendOrder(object order)
        {
            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(order);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Add("x-functions-key", _functionConfig.Key);

                await client.PostAsync(_functionConfig.Url, content);
            }
        }
    }
}
