using System.ComponentModel.DataAnnotations;

namespace Notus.Models.User.Dto
{
    public class RegisterDTO
    {
        [Required]
        [MinLength(2)]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(2)]
        public string Surname { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = null!;

        [Required]
        [MinLength(8)]
        public string ConfirmPassword { get; set; } = null!;

        public string Role { get; set; } = null!;
    }
}
