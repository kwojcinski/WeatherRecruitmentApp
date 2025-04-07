using Core.DTOs;

namespace Abstraction.Interfaces.Services
{
    public interface IWeatherService
    {
        Task<List<WeatherDTO>> GetWeatherLogsAsync();
        Task UpdateWeatherDataAsync();
    }
}
