using System.Net;
using System.Text;
using Web.Services;

namespace Test.Unit;

[TestClass]
public class WeatherServiceTest
{
    private const string NominatimJson = """
        {
            "address": {
                "city": "Buenos Aires",
                "state": "Ciudad Autónoma de Buenos Aires"
            }
        }
        """;

    [TestMethod]
    public async Task Given_ValidResponse_When_GetCurrentWeather_Then_ReturnsFahrenheitAndCelsius()
    {
        var weatherJson = """
        {
            "current": {
                "temperature_2m": 77.0,
                "weather_code": 0
            }
        }
        """;

        var handler = new FakeHttpMessageHandler(new Dictionary<string, string>
        {
            { "open-meteo", weatherJson },
            { "nominatim", NominatimJson }
        });
        var service = new WeatherService(new HttpClient(handler));

        var result = await service.GetCurrentWeatherAsync("-34.6131", "-58.3772");

        Assert.AreEqual(77.0, result.TemperatureFahrenheit, 0.01);
        Assert.AreEqual(25.0, result.TemperatureCelsius, 0.01);
        Assert.AreEqual("Buenos Aires", result.City);
        Assert.AreEqual("Despejado", result.Description);
    }

    [TestMethod]
    public async Task Given_RainWeatherCode_When_GetCurrentWeather_Then_MapsDescriptionCorrectly()
    {
        var weatherJson = """
        {
            "current": {
                "temperature_2m": 50.0,
                "weather_code": 61
            }
        }
        """;

        var handler = new FakeHttpMessageHandler(new Dictionary<string, string>
        {
            { "open-meteo", weatherJson },
            { "nominatim", NominatimJson }
        });
        var service = new WeatherService(new HttpClient(handler));

        var result = await service.GetCurrentWeatherAsync("-34.6131", "-58.3772");

        Assert.AreEqual("Lluvia", result.Description);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public async Task Given_NullCurrentInResponse_When_GetCurrentWeather_Then_ThrowsException()
    {
        var weatherJson = """{ "current": null }""";

        var handler = new FakeHttpMessageHandler(new Dictionary<string, string>
        {
            { "open-meteo", weatherJson },
            { "nominatim", NominatimJson }
        });
        var service = new WeatherService(new HttpClient(handler));

        await service.GetCurrentWeatherAsync("-34.6131", "-58.3772");
    }

    [TestMethod]
    public async Task Given_NominatimReturnsTown_When_GetCurrentWeather_Then_UsesTownAsFallback()
    {
        var weatherJson = """
        {
            "current": {
                "temperature_2m": 60.0,
                "weather_code": 0
            }
        }
        """;

        var nominatimWithTown = """
        {
            "address": {
                "town": "San Isidro",
                "state": "Buenos Aires"
            }
        }
        """;

        var handler = new FakeHttpMessageHandler(new Dictionary<string, string>
        {
            { "open-meteo", weatherJson },
            { "nominatim", nominatimWithTown }
        });
        var service = new WeatherService(new HttpClient(handler));

        var result = await service.GetCurrentWeatherAsync("-34.47", "-58.53");

        Assert.AreEqual("San Isidro", result.City);
    }
}

internal class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly Dictionary<string, string> _responses;

    public FakeHttpMessageHandler(Dictionary<string, string> responses)
    {
        _responses = responses;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var url = request.RequestUri?.Host ?? "";
        var content = _responses.FirstOrDefault(r => url.Contains(r.Key)).Value
            ?? throw new InvalidOperationException($"No fake response configured for: {url}");

        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(content, Encoding.UTF8, "application/json")
        };
        return Task.FromResult(response);
    }
}