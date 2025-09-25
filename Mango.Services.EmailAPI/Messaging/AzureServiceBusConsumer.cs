using System.Text;
using Azure.Messaging.ServiceBus;
using Mango.Services.EmailAPI.Models.DTO;
using Mango.Services.EmailAPI.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json;

namespace Mango.Services.EmailAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string emailCartQueue;

        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        private ServiceBusProcessor _emailcartProcessor;
        public AzureServiceBusConsumer(IConfiguration configuration,EmailService emailService)
        {
            _configuration = configuration;
            _emailService = emailService;
            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailSoppingCartQueue");

            var client = new ServiceBusClient(serviceBusConnectionString);
            _emailcartProcessor = client.CreateProcessor(emailCartQueue);

        }

        public async Task start()
        {
            _emailcartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
            _emailcartProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailcartProcessor.StartProcessingAsync();
        }

        public async Task stop()
        {
           await _emailcartProcessor.StopProcessingAsync();
           await _emailcartProcessor.DisposeAsync();
        }

        private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs args)
        {
            //This is where you recive message
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
           
            CartDTO Objmessage = JsonConvert.DeserializeObject<CartDTO>(body);  
            try
            {
                //TODO - try to log email
                await _emailService.EmailCartAndLog(Objmessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw;

            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

       
    }
}
