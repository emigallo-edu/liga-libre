namespace Web.Models;

public class WeatherInfo
{
    public double Temperature { get; set; }
    public string Description { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
}
