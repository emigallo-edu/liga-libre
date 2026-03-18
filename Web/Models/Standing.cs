namespace Web.Models;

public class Standing
{
    public int TournamentId { get; set; }
    public int ClubId { get; set; }
    public int Win { get; set; }
    public int Draw { get; set; }
    public int Loss { get; set; }
    public Club Club { get; set; } = new();
}
