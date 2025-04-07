using Abstraction.Interfaces.Services;
using Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace WeatherMVCApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet]
        [Route("GetLogs")]
        public async Task<IActionResult> GetLogs()
        {
            var logs = await _weatherService.GetWeatherLogsAsync();
            return Ok(logs);
        }
    }
}
