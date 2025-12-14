using System.Text.Json;
using WeatherAPIWrapperService.DTOs;

namespace WeatherAPIWrapperService.Mappers
{
    public class WeatherMapper
    {
        public static SimpleWeather MapToSimpleWeather(string jsonStringResponse, string city)
        {
            var rawData = JsonSerializer.Deserialize<OpenWeatherMapReponse>(jsonStringResponse,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (rawData == null || rawData.Main == null || rawData.Weather == null)
            {
                throw new Exception("Invalid or incomplete weather data received.");
            }


            return new SimpleWeather
            {
                City = city,
                TemperatureCelsius = rawData.Main.Temp,
                Description = rawData.Weather.FirstOrDefault()?.Description ?? "No description available",
                RetrievedOnUtc = DateTimeOffset.FromUnixTimeSeconds(rawData.Dt).UtcDateTime
            };
        }
    }
}
