using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.ApplicationCore.Model
{
    public class AzureServiceBusConfig
    {
        public string Connection { get; set; }
        public string QueueName { get; set; }
    }
}
