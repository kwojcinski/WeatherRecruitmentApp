using Abstraction.Interfaces.Repositories;
using Abstraction.Interfaces.Services;
using AutoMapper;
using Core.DTOs;
using Core.Models;

namespace Services.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherRepository _weatherRepository;
        private readonly IWeatherApiClient _apiClient;
        private readonly IMapper _mapper;

        public WeatherService(IWeatherRepository weatherRepository, IWeatherApiClient apiClient, IMapper mapper)
        {
            _weatherRepository = weatherRepository;
            _apiClient = apiClient;
            _mapper = mapper;
        }

        public async Task<List<WeatherDTO>> GetWeatherLogsAsync()
        {
            var weatherRecords =  await _weatherRepository.GetAllAsync();
            return _mapper.Map<List<WeatherDTO>>(weatherRecords);
        }

        public async Task UpdateWeatherDataAsync()
        {
         var cities = new List<(string Country, string City)>
            {
                ("UK", "London"),
                ("UK", "Manchester"),
                ("USA", "New York"),
                ("USA", "Los Angeles"),
                ("Poland", "Krakow"),
                ("Poland", "Warsaw")
            };

            foreach (var (country, city) in cities)
            {
                WeatherRecord record = await _apiClient.GetWeatherDataAsync(country, city);

                //WASN'T SURE IF I NEED UPDATE THE CURRENT RECORD OR JUST ADD ANOTHER ONE TO THE TABLE (DID FIRST BECAUSE OF LAST UPDATE TIME IN THE TASK)
                //BECAUSE OF THAT IN THE REACT APP I'M SHOWING MIN/MAX FROM THE API DATA BUT CODE FOR CALCULATING MIN/MAX IS THERE TOO
                var existing = await _weatherRepository.GetRecordByCountryAndCity(country, city);

                if (existing != null)
                {
                    existing.Temperature = record.Temperature;
                    existing.LastUpdate = record.LastUpdate;
                    await _weatherRepository.UpdateAsync(existing);
                }
                else
                {
                    await _weatherRepository.AddAsync(record);
                }
            }
        }
    }
}
