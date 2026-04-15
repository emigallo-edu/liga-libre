using System.ComponentModel.DataAnnotations;

namespace NetWebApi.Model
{
    public class ClubDTOV2
    {
        public ClubDTOV2()
        {
        }

        public int Id { get; set; }

        [Required]
        public string NombreClub { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        [StringLength(150)]
        public string City { get; set; }

        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$")]
        public string Email { get; set; }

        [Range(1, 9999)]
        public int NumberOfPartners { get; set; }

        [Phone]
        public string Phone { get; set; }

        public string? Address { get; set; }

        public string? StadiumName { get; set; }
        [Required]
        public string ContactName { get; set; }
    }
}