using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.ApplicationCore.Interfaces
{
    public interface IOrderSendService
    {
        Task SendOrder(object order);
    }

    public interface IAzureServiceBusSend : IOrderSendService
    {

    }

    public interface IAzureFunctionSend : IOrderSendService
    {

    }
}
