using Microsoft.AspNetCore.Mvc;

namespace ApiShutDown.Controllers;

[ApiController]
[Route("api")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet("GetShortTime")]
    public async Task<string> GetShortTime()
    {
        int i;
        for (i = 0; i < 10; i++)
        {
            await Task.Delay(1000);
            _logger.LogInformation($"GetShortTime Sleep {i}");
        }
        return $"Short time no see {i}!";
    }

    [HttpGet("GetLongTime")]
    public async Task<string> GetLongTime()
    {
        int i;
        for (i = 0; i < 60; i++)
        {
            await Task.Delay(1000);
            _logger.LogInformation($"GetLongTime Sleep {i}");
        }
        return $"Long time no see {i}!";
    }

    [HttpGet("GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
