using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace F1_Fantasy_liga.Models
{
    public class Driver
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public int Number { get; set; }
        public decimal Price { get; set; }

        [ForeignKey("Constructor")]
        public int ConstructorId { get; set; }
        public virtual Constructor Constructor { get; set; }
        public virtual ICollection<RaceResult> RaceResults { get; set; } = new List<RaceResult>();
        public virtual ICollection<FantasyTeam> FantasyTeams { get; set; } = new List<FantasyTeam>();

        [NotMapped]
        public int Points => RaceResults?.Sum(r => r.ScoredPoints) ?? 0;
    }
}
