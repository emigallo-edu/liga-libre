using System.Text.Json.Serialization;
using Web.Models;

namespace Web.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _http;
    private readonly string _unit;

    public WeatherService(HttpClient http)
    {
        _http = http;
        _unit = "fahrenheit";
        //_unit = "celsius";
    }

    public async Task<WeatherInfo> GetCurrentWeatherAsync(string latitude, string longitude)
    {
        var weatherTask = _http.GetFromJsonAsync<OpenMeteoResponse>(
            $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&current=temperature_2m,weather_code&temperature_unit={_unit}");

        var cityTask = GetCityNameAsync(latitude, longitude);

        await Task.WhenAll(weatherTask, cityTask);

        var response = weatherTask.Result;
        if (response?.Current is null)
            throw new InvalidOperationException("No se pudo obtener el clima.");

        return new WeatherInfo
        {
            TemperatureFahrenheit = response.Current.Temperature2m,
            Description = MapWeatherCode(response.Current.WeatherCode),
            City = cityTask.Result
        };
    }

    private async Task<string> GetCityNameAsync(string latitude, string longitude)
    {
        var request = new HttpRequestMessage(HttpMethod.Get,
            $"https://nominatim.openstreetmap.org/reverse?lat={latitude}&lon={longitude}&format=json&accept-language=es");
        request.Headers.UserAgent.ParseAdd("LigaLibre/1.0");

        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<NominatimResponse>();
        return result?.Address?.City
            ?? result?.Address?.Town
            ?? result?.Address?.State
            ?? "Desconocido";
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

internal class NominatimResponse
{
    [JsonPropertyName("address")]
    public NominatimAddress? Address { get; set; }
}

internal class NominatimAddress
{
    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("town")]
    public string? Town { get; set; }

    [JsonPropertyName("state")]
    public string? State { get; set; }
}
