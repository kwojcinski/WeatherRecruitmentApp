using Abstraction.Interfaces.Services;
using Core.DTOs;
using Core.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services.Services
{
    public class WeatherApiClient : IWeatherApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public WeatherApiClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<WeatherRecord> GetWeatherDataAsync(string country, string city)
        {
            string apiKey = _configuration["WeatherApiKey"];
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var openWeatherResponse = JsonSerializer.Deserialize<OpenWeatherResponse>(json, options);

            //WASN'T SURE IF I NEED UPDATE THE CURRENT RECORD OR JUST ADD ANOTHER ONE TO THE TABLE (DID FIRST BECAUSE OF LAST UPDATE TIME IN THE TASK)
            //BECAUSE OF THAT IN THE REACT APP I'M SHOWING MIN/MAX FROM THE API DATA BUT CODE FOR CALCULATING MIN/MAX IS THERE TOO
            return new WeatherRecord
            {
                Country = country,
                City = city,
                Temperature = openWeatherResponse.Main.Temp,
                MinTemperature = openWeatherResponse.Main.Temp_Min,
                MaxTemperature = openWeatherResponse.Main.Temp_Max,
                LastUpdate = DateTime.UtcNow
            };
        }
    }
}
