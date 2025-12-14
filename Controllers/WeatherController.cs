using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WeatherAPIWrapperService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController: ControllerBase
    {
        private readonly Serivces.IWeatherService _weatherService;
        public WeatherController(Serivces.IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }
        [HttpGet("{city}")]
        public async Task<IActionResult> GetWeather(string city)
        {
            if (string.IsNullOrEmpty(city))
            {
                return BadRequest("City name is required.");
            }
            try
            {

            var weather = await _weatherService.GetWeatherAsync(city);
            return Ok(weather);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound($"Weather data for city '{city}' not found.");
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
