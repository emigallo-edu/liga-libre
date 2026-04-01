namespace Web.Models;

public class WeatherInfo
{
    public double TemperatureFahrenheit { get; set; }
    public double TemperatureCelsius => (TemperatureFahrenheit - 32) * 5.0 / 9.0;
    public string Description { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
}