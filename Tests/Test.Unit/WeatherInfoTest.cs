using Web.Models;

namespace Test.Unit;

[TestClass]
public class WeatherInfoTest
{
    [TestMethod]
    public void TemperatureCelsius_WhenFahrenheitIs32_ReturnsZero()
    {
        var weather = new WeatherInfo { TemperatureFahrenheit = 32 };

        Assert.AreEqual(0, weather.TemperatureCelsius, 0.01);
    }

    [TestMethod]
    public void TemperatureCelsius_WhenFahrenheitIs212_Returns100()
    {
        var weather = new WeatherInfo { TemperatureFahrenheit = 212 };

        Assert.AreEqual(100, weather.TemperatureCelsius, 0.01);
    }

    [TestMethod]
    public void TemperatureCelsius_WhenFahrenheitIs77_Returns25()
    {
        var weather = new WeatherInfo { TemperatureFahrenheit = 77 };

        Assert.AreEqual(25, weather.TemperatureCelsius, 0.01);
    }

    [TestMethod]
    public void TemperatureCelsius_WhenFahrenheitIsNegative40_ReturnsNegative40()
    {
        var weather = new WeatherInfo { TemperatureFahrenheit = -40 };

        Assert.AreEqual(-40, weather.TemperatureCelsius, 0.01);
    }
}