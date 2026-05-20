using System.ComponentModel.DataAnnotations;
namespace F1_Fantasy_liga.Models
{
    public class Circuit
    {
        [Key]
        public int Id { get; set; }
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

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
