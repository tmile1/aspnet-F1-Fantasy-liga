using System.ComponentModel.DataAnnotations;

namespace F1_Fantasy_liga.Models.DTO
{
    public class CircuitUpdateDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Country { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        [Range(1, 20)]
        public double Length { get; set; }

        [Range(1, 100)]
        public int NumberOfLaps { get; set; }
    }
}
