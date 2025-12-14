using System.ComponentModel.DataAnnotations;

namespace Shared.DTO.Identity
{
    public class RegisterDTO
    {
        [EmailAddress]
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
    }
}
