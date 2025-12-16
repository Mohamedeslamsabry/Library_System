using Domain_Layer.Exceptions;
using Domain_Layer.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service_Abstraction.Interfaces;
using Shared.DTO.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace Service_Implemention.Service
{
    public class AuthentcationService(UserManager<ApplicationUser> _userManager, IConfiguration _configuration, IEmailSender _emailSender, IConfiguration _config) : IAuthentctionService
    {
        #region Login
        public async Task<UserReturnDTO> LoginAsync(LoginDTO loginDTO)
        {
            var User = await _userManager.FindByEmailAsync(loginDTO.Email) 
                ?? throw new UserrNotFoundException(loginDTO.Email);           

            //Check passowrd
            var IsPassowrdValid = await _userManager.CheckPasswordAsync(User, loginDTO.Password);
            if (IsPassowrdValid)
            {
                return new UserReturnDTO()
                {
                    Email = User.Email!,
                    FirstName = User.FirstName,
                    LastName = User.LastName,
                    Token = await GenerateTokenAsync(User)
                };
            }
            else
            {
                throw new UnauthorizedException();
            }

        }
        #endregion

        #region Register
        public async Task<UserReturnDTO> RegisterAsync(RegisterDTO registerDTO)
        {
            var User = new ApplicationUser()
            {
                Email = registerDTO.Email,
                UserName = registerDTO.UserName,
                PhoneNumber = registerDTO.PhoneNumber,
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName!,
            };

            var Result = await _userManager.CreateAsync(User, registerDTO.Password);

            if (Result.Succeeded)
            {
                return new UserReturnDTO()
                {
                    Email = User.Email!,
                    FirstName = User.FirstName,
                    LastName = User.LastName,
                    Token = await GenerateTokenAsync(User)
                };
            }

            else // Occures ModelState error Example Passowrd not Correct Microsoft configraution
            {
                var Errors = Result.Errors.Select(E => E.Description).ToList();
                throw new BadRequestException(Errors);
            }
        }


        #endregion

        #region Token
        private async Task<string> GenerateTokenAsync(ApplicationUser user)
        {
            #region Payload (Claim)
            var Clamis = new List<Claim>()
            {
                new (ClaimTypes.Email, user.Email!),
                new (ClaimTypes.Name, user.UserName!),
                new (ClaimTypes.NameIdentifier, user.Id),
                new Claim("ss", user.SecurityStamp ?? string.Empty),
                new Claim("jti", Guid.NewGuid().ToString())

            };

            var Roles = await _userManager.GetRolesAsync(user);
            foreach (var role in Roles)
                Clamis.Add(new Claim(ClaimTypes.Role, role));
            #endregion

            var SecritKey = _configuration.GetSection("JWTOptions")["SecritKey"];
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecritKey!));

            var Credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            var Token = new JwtSecurityToken
                            (
                               issuer: _configuration.GetSection("JWTOptions")["Issuer"], //Claim
                               audience: _configuration.GetSection("JWTOptions")["Audience"],//Claim
                               claims: Clamis,//Claim
                               expires: DateTime.Now.AddMinutes(30),//Claim
                               signingCredentials: Credentials
                            );
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
        #endregion

        #region Reset pass && Forget Pass
        public async Task<bool> ResetPasswordLinkAsync(ForgotPasswordRequest model, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(model.Email)) return false;

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null) return false;

            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            if (user is null || !(await _userManager.IsEmailConfirmedAsync(user)))
                return true;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var tokenEncoded = HttpUtility.UrlEncode(token);

            var frontendResetUrl = _config["Frontend:ResetPasswordUrl"]
                                   ?? "https://your-frontend/reset-password";
            var resetUrl = $"{frontendResetUrl}?email={HttpUtility.UrlEncode(model.Email)}&token={tokenEncoded}";

            var subject = "Reset Password";
            var html = $@"
            <p>Hi,</p>
            <p>Click the link below to reset your password:</p>
            <p><a href=""{resetUrl}"">{resetUrl}</a></p>
            <p>If you didn't request this, please ignore.</p>";

            await _emailSender.SendAsync(model.Email, subject, html, ct);
            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordRequest model, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(model.Email) ||
                           string.IsNullOrWhiteSpace(model.Token) ||
                string.IsNullOrWhiteSpace(model.NewPassword))
                return false;

            if (model.NewPassword.Length < 8) return false;

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null) return false;

            var decodedToken = HttpUtility.UrlDecode(model.Token);
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword);
            if (!result.Succeeded) return false;

            await _userManager.UpdateSecurityStampAsync(user);
            return true;
        }
        #endregion

        #region ChangePasswordAsync
        public async Task<bool> ChangePasswordAsync(ClaimsPrincipal userPrincipal, ChangePasswordDto model, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(model.CurrentPassword) ||
                string.IsNullOrWhiteSpace(model.NewPassword))

                return false;

            if (model.NewPassword.Length < 8) 
                return false;


            var user = await _userManager.GetUserAsync(userPrincipal);
            if (user is null) return false;

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);


            if (!result.Succeeded) return false;

            await _userManager.UpdateSecurityStampAsync(user);

            return true;
        }

        #endregion

        #region Check Email
        public async Task<bool> CheckEmailAsync(string email)
        {
            var User = await _userManager.FindByEmailAsync(email);
            if (User is null)
                return false;
            else
                return true;
        }
        #endregion

        #region GetCurrentUserAsync
        public async Task<UserReturnDTO> GetCurrentUserAsync(string email)
        {
            var User = await _userManager.FindByEmailAsync(email) ?? throw new UserrNotFoundException(email);

            return new UserReturnDTO()
            {
                FirstName = User.FirstName,
                LastName = User.LastName,
                Email = User.Email!,
                Token = await GenerateTokenAsync(User)
            };

        }
        #endregion

        #region LogoutAllAsync
        public async Task<bool> LogoutAllAsync(ClaimsPrincipal userPrincipal, CancellationToken ct = default)
        {
            var user = await _userManager.GetUserAsync(userPrincipal);
            if (user is null) return false;

            await _userManager.UpdateSecurityStampAsync(user);
            return true;
        }
        #endregion
    }



    public class SmtpEmailSender : IEmailSender
    {
        private readonly string _from;
        private readonly SmtpClient _client;
        private readonly IConfiguration _config;

        public SmtpEmailSender(IConfiguration config)
        {
            _config = config;
            _from = _config["Smtp:From"] ?? _config["Smtp:User"] ?? "no-reply@yourdomain.com";
        }

        public async Task SendAsync(string to, string subject, string htmlBody, CancellationToken ct = default)
        {
            try
            {
                using var client = new SmtpClient(_config["Smtp:Host"]!)
                {
                    Port = int.Parse(_config["Smtp:Port"] ?? "587"),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_config["Smtp:User"], _config["Smtp:Pass"])
                };

                using var msg = new MailMessage(_from, to)
                {
                    Subject = subject,
                    Body = htmlBody,
                    IsBodyHtml = true
                };

                await client.SendMailAsync(msg);
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"SMTP ERROR: {ex.StatusCode} - {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"INNER: {ex.InnerException.Message}");
                throw; 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MAIL ERROR: {ex.Message}");
                throw;
            }
        }

      
    }



    public class InMemoryRevokedTokenStore : IRevokedTokenStore
    {
        private readonly HashSet<string> _revoked = new();

        public Task RevokeAsync(string jti, DateTime expiresAt)
        {
            _revoked.Add(jti);
            return Task.CompletedTask;
        }

        public Task<bool> IsRevokedAsync(string jti)
            => Task.FromResult(_revoked.Contains(jti));
    }

}
