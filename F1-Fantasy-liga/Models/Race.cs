using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace F1_Fantasy_liga.Models
{
    public class Race
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
        public DateTime RaceDate { get; set; }

        [ForeignKey("Circuit")]
        public int CircuitId { get; set; }
        public virtual Circuit? Circuit { get; set; }
        public virtual ICollection<RaceResult> RaceResults { get; set; } = new List<RaceResult>();

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
