using Core.Models;

namespace Abstraction.Interfaces.Repositories
{
    public interface IWeatherRepository : IRepository<WeatherRecord>
    {
        Task<WeatherRecord?> GetRecordByCountryAndCity(string country, string city);
    }
}
