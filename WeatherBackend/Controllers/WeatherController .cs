using Microsoft.AspNetCore.Mvc;
using WeatherBackend.Models;
using WeatherBackend.Services;

public class WeatherController : ControllerBase
{
    private readonly WeatherService _weatherService;

    public WeatherController(WeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpGet("current/{city}")]
    public async Task<ActionResult<Weather>> GetCurrentWeather(string city)
    {
        var weather = await _weatherService.GetCurrentWeatherAsync(city);
        return Ok(weather);
    }

    [HttpGet("forecast/{city}")]
    public async Task<ActionResult<List<Forecast>>> GetForecast(string city)
    {
        var forecast = await _weatherService.GetFiveDayForecastAsync(city);
        return Ok(forecast);
    }
}