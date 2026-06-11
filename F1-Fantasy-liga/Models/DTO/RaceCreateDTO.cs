using System.ComponentModel.DataAnnotations;

namespace F1_Fantasy_liga.Models.DTO
{
    public class RaceCreateDTO
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public DateTime RaceDate { get; set; }

        [Required]
        public int CircuitId { get; set; }
    }
}
