using System.ComponentModel.DataAnnotations;

namespace Shared.DTO.Identity
{
    public class ForgotPasswordRequest
    {
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; } = null!;
    }

    public class ResetPasswordRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty; 
        public string NewPassword { get; set; } = string.Empty;

    }
}