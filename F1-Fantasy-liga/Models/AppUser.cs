using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace F1_Fantasy_liga.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Surname { get; set; } = string.Empty;

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<FantasyTeam> FantasyTeams { get; set; } = new List<FantasyTeam>();
    }
}
