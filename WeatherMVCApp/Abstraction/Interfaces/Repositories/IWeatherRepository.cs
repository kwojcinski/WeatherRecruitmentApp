using Core.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstraction.Interfaces.Repositories
{
    public interface IWeatherRepository : IRepository<WeatherRecord>
    {
        Task<WeatherRecord?> GetRecordByCountryAndCity(string country, string city);
    }
}
