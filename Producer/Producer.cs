using Microsoft.Azure.ServiceBus;
using System.Text;

namespace Producer
{
    internal class Producer
    {
        private static IQueueClient queueClient;
        private const string ServiceBusConnectionString = "Az service bus > Shared access policy > RootManageSharedAccessKey > Connection string";
        private const string QueueName = "Queue Name";
        public Producer()
        {
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
        }
        public async Task SendMessagesToQueue(int numMessagesToSend)
        {
            for (var i = 0; i < numMessagesToSend; i++)
            {
                try
                {
                    var message = new Message(Encoding.UTF8.GetBytes($"Message {i}"));
                    Console.WriteLine($"Sending message: {Encoding.UTF8.GetString(message.Body)}");
                    await queueClient.SendAsync(message);
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
                }
                await Task.Delay(10);
            }

            Console.WriteLine($"{numMessagesToSend} messages sent.");
            await queueClient.CloseAsync();
        }
    }
}
