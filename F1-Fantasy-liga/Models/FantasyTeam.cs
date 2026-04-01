namespace F1_Fantasy_liga.Models
{
    public class FantasyTeam
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Budget { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<Driver> Drivers { get; set; } = new List<Driver>();
        public int ConstructorId { get; set; }
        public Constructor Constructor { get; set; }
        public int Points { get; set; }

        public int FantasyLeagueId { get; set; }
        public FantasyLeague FantasyLeague { get; set; }
    }
}
