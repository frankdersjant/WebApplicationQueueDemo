using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System.Text.Json;

namespace WebApplicationQueue.Backgroundservice
{
    public class WeatherDataService : BackgroundService
    {
        private readonly ILogger<WeatherDataService> _logger;
        public WeatherDataService(ILogger<WeatherDataService> logger)
        {
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //copied from the weathercontroller - BAD practice!!
            string queueName = "queuedemo";
            string connectionstring = "DefaultEndpointsProtocol=https;AccountName=storageaccounttstfd961e;AccountKey=CkplBShYqFePaOCgDKXmdztA9pklAGdPWFsQyGPpKGaVQ3Jf8eT4QDslABkRU4E8/NdsM+hj5tViAzHdnX9zRA==;EndpointSuffix=core.windows.net";
            QueueClient queueclient = new QueueClient(connectionstring, queueName);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Reading queue");

                QueueMessage[] receivedMessage = await queueclient.ReceiveMessagesAsync();

                if (receivedMessage.Length != 0)
                {
                    foreach (QueueMessage item in receivedMessage)
                    {
                        //cast queuemessag back to object
                        WeatherForecast weatherForecast = JsonSerializer.Deserialize<WeatherForecast>(item.MessageText);

                        //and use the objects for further development in your app!

                        //if we want to delete message:
                       // await queueclient.DeleteMessageAsync(item.MessageId, item.PopReceipt);
                    }
                }

                //poll again after message READ and/or delete
                await Task.Delay(TimeSpan.FromSeconds(5));

            }
        }
    }
}
