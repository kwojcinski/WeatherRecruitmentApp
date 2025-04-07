using Core.Models;

namespace Abstraction.Interfaces
{
    public interface IWeatherService
    {
        Task<WeatherResponse> FetchWeatherAsync();
    }
}
