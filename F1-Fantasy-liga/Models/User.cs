using F1_Fantasy_liga.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace F1_Fantasy_liga.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string Surname { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [StringLength(200)]
        public string PasswordHash { get; set; } = string.Empty;

        [NotMapped]
        [Required]
        [Compare(nameof(PasswordHash), ErrorMessage = "Passwords do not match.")]
        [StringLength(200)]
        public string ConfirmPassword { get; set; } = string.Empty;
        public virtual ICollection<FantasyTeam> FantasyTeams { get; set; } = new List<FantasyTeam>();
        public Role Role { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
