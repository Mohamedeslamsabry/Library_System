using Shared.DTO.Identity;
using Shared.DTO.User;
using System.Security.Claims;

namespace Service_Abstraction.Interfaces
{
    public interface IAuthentctionService
    {
        #region Login
        Task<UserReturnDTO> LoginAsync(LoginDTO loginDTO);
        #endregion

        #region Register
        Task<UserReturnDTO> RegisterAsync(RegisterDTO registerDTO);

        #endregion

        #region Reset Passowrd

        Task<bool> ResetPasswordLinkAsync(ForgotPasswordRequest model, CancellationToken ct = default);
        Task<bool> ResetPasswordAsync(ResetPasswordRequest model, CancellationToken ct = default);

        #endregion

        #region ChangePasswordAsync
        Task<bool> ChangePasswordAsync(ClaimsPrincipal userPrincipal, ChangePasswordDto model, CancellationToken ct = default);

        #endregion

        #region CheckEmailAsync
        Task<bool> CheckEmailAsync(string email);

        #endregion

        #region GetCurrentUserAsync
        Task<UserReturnDTO> GetCurrentUserAsync(string email);
        #endregion

        #region LogoutAllAsync
        Task<bool> LogoutAllAsync(ClaimsPrincipal userPrincipal, CancellationToken ct = default);

        #endregion
    }

    public interface IEmailSender
    {
        Task SendAsync(string to, string subject, string htmlBody, CancellationToken ct = default);
    }


    public interface IRevokedTokenStore
    {
        Task RevokeAsync(string jti, DateTime expiresAt);
        Task<bool> IsRevokedAsync(string jti);
    }

}
