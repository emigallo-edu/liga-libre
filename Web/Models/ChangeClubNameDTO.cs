using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class ChangeClubNameDTO
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Name { get; set; } = string.Empty;
}
