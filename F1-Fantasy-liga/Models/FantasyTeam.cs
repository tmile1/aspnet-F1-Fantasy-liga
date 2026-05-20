using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace F1_Fantasy_liga.Models
{
    public class FantasyTeam
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
        public decimal Budget { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Driver> Drivers { get; set; } = new List<Driver>();

        [ForeignKey("Constructor")]
        public int ConstructorId { get; set; }
        public virtual Constructor? Constructor { get; set; }

        [NotMapped]
        public int Points => (Drivers?.Sum(d => d.Points) ?? 0) + (Constructor?.Points/2 ?? 0);

        [ForeignKey("FantasyLeague")]
        public int FantasyLeagueId { get; set; }
        public virtual FantasyLeague? FantasyLeague { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
