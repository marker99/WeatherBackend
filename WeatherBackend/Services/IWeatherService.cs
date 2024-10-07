using WeatherBackend.Models;

namespace WeatherBackend.Services
{
    public interface IWeatherService
    {
        /// <summary>
        /// Get the current weather for a city
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        Task<Weather> GetCurrentWeatherAsync(string city);

        /// <summary>
        /// Get the five day forecast for a city
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        Task<List<Forecast>> GetFiveDayForecastAsync(string city);
    }
}
