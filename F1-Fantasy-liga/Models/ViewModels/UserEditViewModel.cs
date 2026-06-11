using System.ComponentModel.DataAnnotations;

namespace F1_Fantasy_liga.Models.ViewModels
{
    public class UserEditViewModel
    {
        [Required]
        public string Id { get; set; } = string.Empty;

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

        [StringLength(200)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [StringLength(200)]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string? ConfirmPassword { get; set; }

        [Required]
        public string Role { get; set; } = string.Empty;
    }
}
