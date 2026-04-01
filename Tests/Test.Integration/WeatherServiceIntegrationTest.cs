using Web.Services;

namespace Test.Integration;

[TestClass]
public class WeatherServiceIntegrationTest
{
    private WeatherService _service = null!;
    private const string LATITUDE_BSAS = "-34.6131";
    private const string LONGITUDE_BSAS = "-58.3772";
    private const string LATITUDE_MADRID = "40.4165";
    private const string LONGITUDE_MADRID = "-3.70256";

    [TestInitialize]
    public void Setup()
    {
        var httpClient = new HttpClient();
        _service = new WeatherService(httpClient);
    }

    [TestMethod]
    public async Task Given_BuenosAiresCoordinates_When_GetCurrentWeather_Then_CityIsResolvedFromCoordinates()
    {
        var result = await _service.GetCurrentWeatherAsync(LATITUDE_BSAS, LONGITUDE_BSAS);

        Assert.IsFalse(string.IsNullOrEmpty(result.City),
            "La ciudad deberia resolverse desde las coordenadas");
        StringAssert.Contains(result.City, "Buenos Aires",
            $"Se esperaba Buenos Aires pero se obtuvo: {result.City}");
    }

    [TestMethod]
    public async Task Given_BuenosAiresCoordinates_When_GetCurrentWeather_Then_FahrenheitIsInReasonableRange()
    {
        var result = await _service.GetCurrentWeatherAsync(LATITUDE_BSAS, LONGITUDE_BSAS);

        Assert.IsTrue(result.TemperatureFahrenheit > 32 && result.TemperatureFahrenheit < 104,
            $"Temperatura en Fahrenheit fuera de rango razonable: {result.TemperatureFahrenheit}");
    }

    [TestMethod]
    public async Task Given_BuenosAiresCoordinates_When_GetCurrentWeather_Then_CelsiusIsInReasonableRange()
    {
        var result = await _service.GetCurrentWeatherAsync(LATITUDE_BSAS, LONGITUDE_BSAS);

        Assert.IsTrue(result.TemperatureCelsius > 0 && result.TemperatureCelsius < 40,
            $"Temperatura en Celsius fuera de rango razonable: {result.TemperatureCelsius}");
    }

    [TestMethod]
    public async Task Given_BuenosAiresCoordinates_When_GetCurrentWeather_Then_DescriptionIsNotEmpty()
    {
        var result = await _service.GetCurrentWeatherAsync(LATITUDE_BSAS, LONGITUDE_BSAS);

        Assert.IsFalse(string.IsNullOrEmpty(result.Description),
            "La descripcion del clima no deberia estar vacia");
    }
}