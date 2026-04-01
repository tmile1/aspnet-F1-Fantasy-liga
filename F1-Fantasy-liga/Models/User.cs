using F1_Fantasy_liga.Models.Enums;

namespace F1_Fantasy_liga.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public List<FantasyTeam> FantasyTeams { get; set; } = new List<FantasyTeam>();
        public Role Role { get; set; }
    }
}
