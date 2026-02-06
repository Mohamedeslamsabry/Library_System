using System.ComponentModel.DataAnnotations;

namespace Shared.DTO.Identity
{
    public class LoginDTO
    {
        [EmailAddress]
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
