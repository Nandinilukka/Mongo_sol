using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Mango.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Mango.ServiceBus
{
    public class MessageBus : IMessageBus
    {
        //private string ConnectionString = "Endpoint=sb://masteryweb.servicebus.windows.net/;SharedAccessKeyName=RootManagerSharedAccesKey;SharedAccessKey=GehbysS7lhlAQFSzrhQo/BYU+VT7WdnY/+ASbEm191A=;EntityPath=emailshoppingcart";

        private readonly IConfiguration _configuration;
        public MessageBus (IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task PublishMessage(object message, string connectionKey, string Topic_Queue_NAme)
        {
            var ConnectionString = _configuration[$"ServiceBusConnections:{connectionKey}"];
            await using var client = new ServiceBusClient(ConnectionString);
            ServiceBusSender sender = client.CreateSender(Topic_Queue_NAme);
            var jsonMessage = JsonConvert.SerializeObject(message);
            ServiceBusMessage finalmessage = new ServiceBusMessage
                (Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString(), //it is an identifier
            };
            await sender.SendMessageAsync(finalmessage);
            await sender.DisposeAsync();
        }
    }
}


