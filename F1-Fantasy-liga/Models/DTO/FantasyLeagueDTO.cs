using F1_Fantasy_liga.Models.Enums;

namespace F1_Fantasy_liga.Models.DTO
{
    public class FantasyLeagueDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeagueType LeagueType { get; set; }
    }
}
