namespace Mango.Services.EmailAPI.Messaging
{
    public interface IAzureServiceBusConsumer
    {
        Task start();
        Task stop();
    }
}
