using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WeatherAPIWrapperService.Configurations;
using WeatherAPIWrapperService.DTOs;
using WeatherAPIWrapperService.Mappers;

namespace WeatherAPIWrapperService.Serivces
{
    public class WeatherService:  IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IDistributedCache _cache;
        private readonly OpenWeatherMapSettings _weatherSettings;
        private readonly CacheConfig _cacheConfig;

        // حقن IOptions<T>
        public WeatherService(
            HttpClient httpClient,
            IDistributedCache cache,
            IOptions<OpenWeatherMapSettings> weatherOptions,
            IOptions<CacheConfig> cacheOptions)
        {
            _httpClient = httpClient;
            _cache = cache;

            _weatherSettings = weatherOptions.Value;
            _cacheConfig = cacheOptions.Value;
        }

        public async Task<SimpleWeather> GetWeatherAsync(string city)
        {
            string url = $"{_weatherSettings.BaseUrl}?q={city}&appid={_weatherSettings.ApiKey}&units={_weatherSettings.Unit}";

            var cacheOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(_cacheConfig.WeatherExpirationMinutes));

            var cacheKey = $"weather:{city.ToLowerInvariant()}";

            var cachedWeatherJson = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedWeatherJson))
            {
                try
                {

                    return JsonSerializer.Deserialize<SimpleWeather>(cachedWeatherJson);
                }

                catch (JsonException) {
                }
            }

            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            var weatherData = WeatherMapper.MapToSimpleWeather(responseContent, city);

            var jsonCacheData = JsonSerializer.Serialize(weatherData);

            await _cache.SetStringAsync(cacheKey, jsonCacheData, cacheOptions);

            return weatherData;
        }
    }
}
