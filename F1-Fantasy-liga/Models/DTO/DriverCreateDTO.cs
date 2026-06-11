using System.ComponentModel.DataAnnotations;

namespace F1_Fantasy_liga.Models.DTO
{
    public class DriverCreateDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Surname { get; set; } = string.Empty;

        [Range(1, 99)]
        public int Number { get; set; }

        [Range(0.1, 1000)]
        public decimal Price { get; set; }

        [Required]
        public int ConstructorId { get; set; }
    }
}
