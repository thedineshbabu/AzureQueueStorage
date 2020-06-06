using System; // Namespace for Console output
using System.Configuration; // Namespace for ConfigurationManager
using System.Threading.Tasks; // Namespace for Task
using Azure.Storage.Queues; // Namespace for Queue storage types
using Azure.Storage.Queues.Models; // Namespace for PeekedMessage
namespace AzureStorageQueue
{
    class Program
    {
        // Get the connection string from app settings
    static string connectionString = "DefaultEndpointsProtocol=https;AccountName=samessagequeue;AccountKey=yaO4s6RjqxYXHsX7XK3ZMyKdOT/k/qzaOLtaQ29ZhBrboNnai12ZDS3fr9YRj1JvUeq1/ZOBxTyC/3IUah4hbg==;EndpointSuffix=core.windows.net";

   
        static void Main(string[] args)
        {
            QueueClient queueClient = new QueueClient(connectionString, "qmessaging");
            queueClient.CreateIfNotExists();

            if (queueClient.Exists())
            {
                queueClient.SendMessage("Sample_Message " + DateTime.Now.Millisecond);
                PeekedMessage[] peekedMessage = queueClient.PeekMessages();

                // Display the message
                Console.WriteLine($"Peeked message: '{peekedMessage[0].MessageText}'");

                // QueueMessage[] retrievedMessage = queueClient.ReceiveMessages();
                // Process (i.e. print) the message in less than 30 seconds
                // Console.WriteLine($"De-queued message: '{retrievedMessage[0].MessageText}'");

                // Delete the message
                // queueClient.DeleteMessage(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);

                QueueProperties properties = queueClient.GetProperties();

                // Retrieve the cached approximate message count.
                int cachedMessagesCount = properties.ApproximateMessagesCount;

                // Display number of messages.
                Console.WriteLine($"Number of messages in queue: {cachedMessagesCount}");

                QueueMessage[] receivedMessages = queueClient.ReceiveMessages(20, TimeSpan.FromMinutes(5));

                foreach (QueueMessage message in receivedMessages)
                {
                    // Process (i.e. print) the messages in less than 5 minutes
                    Console.WriteLine($"De-queued message: '{message.MessageText}'");

                    // Delete the message
                    queueClient.DeleteMessage(message.MessageId, message.PopReceipt);
                }
            } 
        }
    }
}
