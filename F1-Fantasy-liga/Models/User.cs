using F1_Fantasy_liga.Models.Enums;
using System.ComponentModel.DataAnnotations;
namespace F1_Fantasy_liga.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public virtual ICollection<FantasyTeam> FantasyTeams { get; set; } = new List<FantasyTeam>();
        public Role Role { get; set; }
    }
}
