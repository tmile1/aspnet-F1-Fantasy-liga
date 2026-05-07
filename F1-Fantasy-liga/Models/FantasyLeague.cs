using F1_Fantasy_liga.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace F1_Fantasy_liga.Models
{
    public class FantasyLeague
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual ICollection<FantasyTeam> FantasyTeams { get; set; } = new List<FantasyTeam>();
        public LeagueType LeagueType { get; set; }
    }
}
