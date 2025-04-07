using Abstraction.Interfaces.Repositories;
using Core.Contexts;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Services.Repositories
{
    public class WeatherRepository : Repository<WeatherRecord>, IWeatherRepository
    {
        public WeatherRepository(WeatherContext context) : base(context)
        {
        }

        public async Task<WeatherRecord?> GetRecordByCountryAndCity(string country, string city)
        {
            return await _context.WeatherRecords.FirstOrDefaultAsync(w => w.Country == country && w.City == city);
        }
    }
}
