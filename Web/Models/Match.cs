namespace Web.Models;

public class Match
{
    public int Id { get; set; }
    public int TournamentId { get; set; }
    public DateTime Date { get; set; }
    public int LocalClubId { get; set; }
    public Club LocalClub { get; set; } = new();
    public int VisitingClubId { get; set; }
    public Club VisitingClub { get; set; } = new();
}
