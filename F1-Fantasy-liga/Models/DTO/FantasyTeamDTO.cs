namespace F1_Fantasy_liga.Models.DTO
{
    public class FantasyTeamDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Budget { get; set; }
        public UserSummaryDTO? User { get; set; }
        public ConstructorSummaryDTO? Constructor { get; set; }
        public FantasyLeagueSummaryDTO? FantasyLeague { get; set; }
    }
}
