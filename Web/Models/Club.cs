namespace Web.Models;

public class Club
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime Birthday { get; set; }
    public string City { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int NumberOfPartners { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? StadiumName { get; set; }
    public Stadium? Stadium { get; set; }
}
