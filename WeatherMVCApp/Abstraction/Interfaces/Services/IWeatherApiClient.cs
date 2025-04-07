using Core.Models;

namespace Abstraction.Interfaces.Services
{
    public interface IWeatherApiClient
    {
        Task<WeatherRecord> GetWeatherDataAsync(string country, string city);
    }
}
