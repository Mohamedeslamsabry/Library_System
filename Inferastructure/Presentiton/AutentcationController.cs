using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_Abstraction.Interfaces;
using Shared.DTO.Identity;
using Shared.DTO.User;
using System.Security.Claims;

namespace Presentiton
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutentcationController(IAuthentctionService _authentctionService) : ControllerBase
    {
        #region Login
        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var Result = await _authentctionService.LoginAsync(loginDTO);
            return Ok(Result);
        }
        #endregion

        #region Register
        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO register)
        {
            var Result = await _authentctionService.RegisterAsync(register);
            return Ok(Result);
        }
        #endregion

        #region Forget Password

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest model, CancellationToken ct)
        {
            var ok = await _authentctionService.ResetPasswordLinkAsync(model, ct);
            // رد عام دائمًا (لا نكشف وجود الإيميل)
            return Ok(new { message = "If the email address is correct, you will receive a link to reset your password." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest model, CancellationToken ct)
        {
            var ok = await _authentctionService.ResetPasswordAsync(model, ct);
            if (!ok) return BadRequest(new { message = "The token is invalid or expired." });
            return Ok(new { message = "The password was successfully set." });
        }



        #endregion

        #region ChangePassword
        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model, CancellationToken ct)
        {
            // تحقق بسيط للمدخلات
            if (string.IsNullOrWhiteSpace(model.CurrentPassword) ||
                string.IsNullOrWhiteSpace(model.NewPassword))
            {
                return BadRequest(new { message = "CurrentPassword و NewPassword مطلوبتان." });
            }

            if (model.NewPassword.Length < 8)
            {
                return BadRequest(new { message = "كلمة المرور الجديدة يجب أن تكون 8 أحرف على الأقل." });
            }

            var ok = await _authentctionService.ChangePasswordAsync(User, model, ct);
            if (!ok)
                return BadRequest(new { message = "فشل تغيير كلمة المرور. تأكد من كلمة المرور الحالية أو سياسة كلمة المرور." });

            return Ok(new { message = "تم تغيير كلمة المرور بنجاح." });


        }
        #endregion

        #region CheckEmail
        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {
            var Result = await _authentctionService.CheckEmailAsync(email);
            return Ok(Result);
        }
        #endregion

        #region Get Current User
        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var Result = await _authentctionService.GetCurrentUserAsync(Email!);
            return Ok(Result);
        }
        #endregion

        #region LogoutAll
        [Authorize(Roles ="SuperAdmin")]
        [HttpPost("logout-all")]
        public async Task<IActionResult> LogoutAll(CancellationToken ct)
        {
            var ok = await _authentctionService.LogoutAllAsync(User, ct);
            if (!ok) return Unauthorized(new { message = "المستخدم غير معروف." });

            return Ok(new { message = "تم تسجيل الخروج من كل الجلسات. الرجاء حذف التوكين الحالي." });
        }
        #endregion

        #region Logout
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromServices] IRevokedTokenStore revokedStore)
        {
            var jti = User.FindFirst("jti")?.Value;
            if (string.IsNullOrEmpty(jti))
                return BadRequest(new { message = "JTI claim is missing." });

            // استخرج وقت انتهاء التوكن الحالي (exp) لو حابب تنظّف بعده
            var expUnix = User.FindFirst("exp")?.Value;

            var expiresAt = DateTime.UtcNow.AddHours(2);
            if (long.TryParse(expUnix, out var expVal))
                expiresAt = DateTimeOffset.FromUnixTimeSeconds(expVal).UtcDateTime;

            await revokedStore.RevokeAsync(jti, expiresAt);
            return Ok(new { message = "تم تسجيل الخروج من الجلسة الحالية." });
        }
        #endregion

    }
}