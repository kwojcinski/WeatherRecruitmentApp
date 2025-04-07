using Abstraction.Interfaces;
using Core.Models;
using Microsoft.Extensions.Configuration;

namespace Services.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public WeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<WeatherResponse> FetchWeatherAsync()
        {
            var apiKey = _configuration["WeatherApiKey"];
            var url = $"https://api.openweathermap.org/data/2.5/weather?q=London&appid={apiKey}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string rawJson = await response.Content.ReadAsStringAsync();
            return new WeatherResponse { RawJson = rawJson };
        }
    }
}
