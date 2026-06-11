using System.ComponentModel.DataAnnotations;

namespace F1_Fantasy_liga.Models.DTO
{
    public class ConstructorCreateDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string Nationality { get; set; } = string.Empty;

        public DateTime? FoundedDate { get; set; }
    }
}
