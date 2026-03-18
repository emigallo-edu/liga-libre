using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class ClubDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string NombreClub { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de fundacion es obligatoria")]
    public DateTime Birthday { get; set; } = DateTime.Now;

    [StringLength(150)]
    public string City { get; set; } = string.Empty;

    [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Email invalido")]
    public string Email { get; set; } = string.Empty;

    [Range(1, 9999, ErrorMessage = "Debe estar entre 1 y 9999")]
    public int NumberOfPartners { get; set; }

    public string Phone { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? StadiumName { get; set; }
}
