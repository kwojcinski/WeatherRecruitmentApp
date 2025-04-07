using Abstraction.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class WeatherUpdateService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<WeatherUpdateService> _logger;

        public WeatherUpdateService(IServiceScopeFactory scopeFactory, ILogger<WeatherUpdateService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Weather update service started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Create a new scope for each update operation.
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        // Resolve the scoped service from the scope.
                        var weatherService = scope.ServiceProvider.GetRequiredService<IWeatherService>();
                        await weatherService.UpdateWeatherDataAsync();
                    }

                    _logger.LogInformation("Weather data updated at {Time}", DateTime.UtcNow);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating weather data.");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
