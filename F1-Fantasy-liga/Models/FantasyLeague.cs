using F1_Fantasy_liga.Models.Enums;

namespace F1_Fantasy_liga.Models
{
    public class FantasyLeague
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<FantasyTeam> FantasyTeams { get; set; } = new List<FantasyTeam>();
        public LeagueType LeagueType { get; set; }
    }
}
