using System.ComponentModel.DataAnnotations;

namespace Model.Entities
{
    public class Stadium
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        public int ClubId { get; set; }

        /// <summary>
        /// Si la capacidad del estadio es mayor a 1000, se debe hacer JOIN con la tabla Regulations
        /// </summary>
        public string? Aux { get; set; }

        public Club? Club { get; set; }
    }
}