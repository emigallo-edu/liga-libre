using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class RegisterMatchDTO
{
    [Required]
    public int StandingId { get; set; }

    [Required]
    public int Matchid { get; set; }

    [Required]
    public int LocalClubGoals { get; set; }

    [Required]
    public int VisitingClubGoals { get; set; }
}
