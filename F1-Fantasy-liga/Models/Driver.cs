using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace F1_Fantasy_liga.Models
{
    public class Driver
    {
        [Key]
        public int Id { get; set; }
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

        [ForeignKey("Constructor")]
        public int ConstructorId { get; set; }
        public virtual Constructor? Constructor { get; set; }
        public virtual ICollection<RaceResult> RaceResults { get; set; } = new List<RaceResult>();
        public virtual ICollection<FantasyTeam> FantasyTeams { get; set; } = new List<FantasyTeam>();

        [NotMapped]
        public int Points => RaceResults?.Sum(r => r.ScoredPoints) ?? 0;

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
