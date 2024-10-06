using Newtonsoft.Json;

namespace WeatherBackend.Models
{
    public class Forecast
    {
        public string DateTime { get; set; }

        public double HighTemp { get; set; } 
        public double LowTemp { get; set; }

        public List<Weather>? HourlyForecast { get; set; }  // List of hourly weather data for that day
    }
}
