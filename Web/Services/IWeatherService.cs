using Web.Models;

namespace Web.Services;

public interface IWeatherService
{
    Task<WeatherInfo> GetCurrentWeatherAsync(string latitude, string longitude);
}