using System.Buffers.Text;

namespace WeatherAPIWrapperService.Configurations
{
    public class OpenWeatherMapSettings
    {
        public string BaseUrl { get; set; }
        public string ApiKey { get; set; }

        public string Unit {  get; set; }
    }
}
