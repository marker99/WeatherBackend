using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeatherBackend.Models;

namespace WeatherBackend.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public WeatherService(HttpClient httpClient, string apiKey)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;
        }

        public async Task<Weather> GetCurrentWeatherAsync(string city)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://api.openweathermap.org/data/2.5/weather?q={city}&units=metric&appid={_apiKey}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var currentWeather = JsonConvert.DeserializeObject<dynamic>(json);

                return new Weather
                {
                    Temperature = currentWeather.main.temp,
                    Description = currentWeather.weather[0].description,
                    Humidity = currentWeather.main.humidity,
                    WindSpeed = currentWeather.wind.speed,
                    Time = DateTimeOffset.FromUnixTimeSeconds((long)currentWeather.dt).DateTime
                };
            }
            catch (HttpRequestException e)
            {
                throw new Exception($"Error fetching weather data for {city}: {e.Message}", e);
            }
        }

        public async Task<List<Forecast>> GetFiveDayForecastAsync(string city)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://api.openweathermap.org/data/2.5/forecast?q={city}&units=metric&appid={_apiKey}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var forecastData = JsonConvert.DeserializeObject<dynamic>(json);

                var forecasts = new List<Forecast>();
                var groupedForecasts = ((IEnumerable<dynamic>)forecastData.list)
                    .GroupBy(item => DateTime.Parse((string)item.dt_txt).Date);

                foreach (var group in groupedForecasts)
                {
                    var dayForecast = new Forecast
                    {
                        DateTime = group.Key.ToString("yyyy-MM-dd"),
                        HighTemp = group.Max(item => (double)item.main.temp_max),
                        LowTemp = group.Min(item => (double)item.main.temp_min),
                        HourlyForecast = group.Select(item => new Weather
                        {
                            Temperature = (float)item.main.temp,
                            Description = (string)item.weather[0].description,
                            Humidity = (int)item.main.humidity,
                            WindSpeed = (float)item.wind.speed,
                            Time = DateTime.Parse((string)item.dt_txt)
                        }).ToList()
                    };
                    forecasts.Add(dayForecast);
                }

                return forecasts.Take(5).ToList();
            }
            catch (HttpRequestException e)
            {
                throw new Exception($"Error fetching forecast data for {city}: {e.Message}", e);
            }
        }
    }
}