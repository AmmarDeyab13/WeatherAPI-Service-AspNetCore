using WeatherAPIWrapperService.DTOs;

namespace WeatherAPIWrapperService.Serivces
{
    public interface IWeatherService
    {
        public Task<SimpleWeather> GetWeatherAsync(string city);
    }
}
