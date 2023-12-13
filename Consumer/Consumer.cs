using Microsoft.Azure.ServiceBus;
using System.Text;

namespace Consumer
{
    internal class Consumer
    {
        private static IQueueClient queueClient;
        private const string ServiceBusConnectionString = "Az service bus > Shared access policy > RootManageSharedAccessKey > Connection string";
        private const string QueueName = "Queue Name";
        public Consumer()
        {
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
        }
        public void ReceiveMessages()
        {
            try
            {
                queueClient.RegisterMessageHandler(
                    async (message, token) =>
                    {
                        Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");

                        await queueClient.CompleteAsync(message.SystemProperties.LockToken);
                    },
                    new MessageHandlerOptions(exceptionReceivedEventArgs =>
                    {
                        Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
                        return Task.CompletedTask;
                    })
                    { MaxConcurrentCalls = 1, AutoComplete = false });
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
            }
            queueClient.CloseAsync();
        }
    }
}
