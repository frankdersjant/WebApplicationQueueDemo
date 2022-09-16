using Azure.Storage.Queues;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace WebApplicationQueue.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost(Name ="PostWeatherData)")]
        public async Task Post([FromBody] WeatherForecast weatherForecast)
        {
            //DEMO purposes ONLY!!!
            string queueName = "queuedemo";
            string connectionstring = "DefaultEndpointsProtocol=https;AccountName=storageaccounttstfd961e;AccountKey=CkplBShYqFePaOCgDKXmdztA9pklAGdPWFsQyGPpKGaVQ3Jf8eT4QDslABkRU4E8/NdsM+hj5tViAzHdnX9zRA==;EndpointSuffix=core.windows.net";
            QueueClient queueclient = new QueueClient(connectionstring, queueName);

            var weather = JsonSerializer.Serialize(weatherForecast);

            //await queueclient.SendMessageAsync(datapacket, visibility timeout, TTL));
            //await queueclient.SendMessageAsync(weather, TimeSpan.FromSeconds(8), TimeSpan.FromSeconds(15));

            await queueclient.SendMessageAsync(weather);
        }
    }
}