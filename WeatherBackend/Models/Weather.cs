using Newtonsoft.Json;

namespace WeatherBackend.Models
{
    public class Weather
    {
        
        public float Temperature { get; set; }
        public string? Description { get; set; }
        public int Humidity { get; set; }
        public float WindSpeed { get; set; }

        public DateTime? Time { get; set; }
    }
}
