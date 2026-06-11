using System.ComponentModel.DataAnnotations;

namespace F1_Fantasy_liga.Models.DTO
{
    public class FantasyTeamCreateDTO
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public decimal Budget { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public int ConstructorId { get; set; }

        [Required]
        public int FantasyLeagueId { get; set; }
    }
}
