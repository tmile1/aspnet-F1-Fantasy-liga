using System.ComponentModel.DataAnnotations;
using F1_Fantasy_liga.Models.Enums;

namespace F1_Fantasy_liga.Models.DTO
{
    public class UserUpdateDTO
    {
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
        public string Password { get; set; } = string.Empty;

        public Role Role { get; set; }
    }
}
