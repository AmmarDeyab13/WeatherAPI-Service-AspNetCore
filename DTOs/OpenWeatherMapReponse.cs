namespace WeatherAPIWrapperService.DTOs
{
    public class OpenWeatherMapReponse
    {
        public MainInfo Main { get; set; }

        
        public List<WeatherInfo> Weather { get; set; }

        public string Name { get; set; }

       
        public long Dt { get; set; }
    }
}
