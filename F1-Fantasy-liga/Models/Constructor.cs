using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace F1_Fantasy_liga.Models
{
    public class Constructor
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(100)]
        public string Nationality { get; set; } = string.Empty;
        public DateTime? FoundedDate { get; set; }
        [StringLength(300)]
        public string ImagePath { get; set; } = string.Empty;

        [NotMapped]
        public int Points => Drivers?.Sum(d => d.Points) ?? 0;
        public virtual ICollection<Driver> Drivers { get; set; } = new List<Driver>();

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
