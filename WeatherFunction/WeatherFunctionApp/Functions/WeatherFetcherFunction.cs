using Abstraction.Interfaces;
using Core.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Diagnostics.Metrics;

namespace WeatherFunctionApp.Functions
{
    public class WeatherFetchFunction
    {
        private readonly IWeatherService _weatherService;
        private readonly IStorageService _storageService;

        public WeatherFetchFunction(IWeatherService weatherService, IStorageService storageService)
        {
            _weatherService = weatherService;
            _storageService = storageService;
        }

        [Function("WeatherFetcherFunction")]
        public async Task Run(
             [TimerTrigger("0 */1 * * * *")] TimerInfo myTimer,
             FunctionContext context)
        {
            var logger = context.GetLogger("WeatherFetcherFunction");
            logger.LogInformation($"WeatherFetcherFunction executed at: {DateTime.UtcNow}");

            WeatherResponse weatherResponse = null;
            bool success = false;

            try
            {
                weatherResponse = await _weatherService.FetchWeatherAsync();
                success = true;
                logger.LogInformation($"Successfully fetched weather for London");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error fetching weather data for London");
            }

            var logEntry = new WeatherLog
            {
                PartitionKey = "WeatherLog",
                RowKey = Guid.NewGuid().ToString(),
                TimestampUtc = DateTime.UtcNow,
                Success = success,
                Message = success
                    ? $"Fetch succeeded for London"
                    : $"Fetch failed for London"
            };

            await _storageService.InsertLogAsync(logEntry);

            if (weatherResponse != null)
            {
                await _storageService.UploadPayloadAsync(logEntry.RowKey, weatherResponse.RawJson);
            }
        }
    }
}
