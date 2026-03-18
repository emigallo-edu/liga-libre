namespace Web.Models;

public class Tournament
{
    public int Id { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public List<Standing> Standings { get; set; } = new();
    public List<Match> Matches { get; set; } = new();
}
