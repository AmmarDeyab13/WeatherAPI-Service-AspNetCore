namespace WeatherAPIWrapperService.DTOs
{
    public class SimpleWeather
    {
        public string City { get; set; }
        public double TemperatureCelsius { get; set; }
        public string Description { get; set; }
        public DateTime RetrievedOnUtc { get; set; } 
    }
}
