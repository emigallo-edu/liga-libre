using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Web.Models;

namespace Web.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _http;

    public WeatherService(HttpClient http)
    {
        _http = http;
    }

    public async Task<WeatherInfo> GetCurrentWeatherAsync()
    {
        var response = await _http.GetFromJsonAsync<OpenMeteoResponse>(
            "https://api.open-meteo.com/v1/forecast?latitude=-34.6131&longitude=-58.3772&current=temperature_2m,weather_code");

        if (response?.Current is null)
            throw new InvalidOperationException("No se pudo obtener el clima.");

        return new WeatherInfo
        {
            Temperature = response.Current.Temperature2m,
            Description = MapWeatherCode(response.Current.WeatherCode),
            City = "Buenos Aires"
        };
    }

    private static string MapWeatherCode(int code) => code switch
    {
        0 => "Despejado",
        1 => "Principalmente despejado",
        2 => "Parcialmente nublado",
        3 => "Nublado",
        45 or 48 => "Niebla",
        51 or 53 or 55 => "Llovizna",
        61 or 63 or 65 => "Lluvia",
        71 or 73 or 75 => "Nieve",
        80 or 81 or 82 => "Chubascos",
        95 => "Tormenta",
        96 or 99 => "Tormenta con granizo",
        _ => "Desconocido"
    };
}

internal class OpenMeteoResponse
{
    [JsonPropertyName("current")]
    public OpenMeteoCurrent? Current { get; set; }
}

internal class OpenMeteoCurrent
{
    [JsonPropertyName("temperature_2m")]
    public double Temperature2m { get; set; }

    [JsonPropertyName("weather_code")]
    public int WeatherCode { get; set; }
}
