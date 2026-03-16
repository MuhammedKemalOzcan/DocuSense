using DocuSense.Application.Dtos.AuthenticationDto;
using DocuSense.Application.Services.Authentication;
using Microsoft.AspNetCore.Identity;

namespace DocuSense.Infrastructure.Services.Authentication
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenService _tokenService;

        public IdentityService(UserManager<IdentityUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<AuthResultDto> LoginAsync(LoginDto loginData)
        {
            var user = await _userManager.FindByEmailAsync(loginData.Email);
            if (user == null)
            {
                return new AuthResultDto { Succeed = false, Error = "Email veya şifre hatalı!" };
            }

            var signInResult = await _userManager.CheckPasswordAsync(user, loginData.Password);

            if (!signInResult)
            {
                return new AuthResultDto { Succeed = false, Error = "Email veya şifre hatalı" };
            }

            var token = _tokenService.GenerateAccessToken(user.Id, user.Email);

            var authResult = new AuthResultDto
            {
                Succeed = true,
                UserId = user.Id,
                AccessToken = token.AccessToken,
                Expiration = token.Expiration
            };

            return authResult;
        }

        public async Task<AuthResultDto> RegisterAsync(RegisterDto registerData)
        {
            var existing = await _userManager.FindByEmailAsync(registerData.Email);
            if (existing != null)
            {
                return new() { Succeed = false, Error = "Bu email zaten kullanılmış" };
            }

            IdentityUser user = new IdentityUser
            {
                UserName = registerData.Email.Split("@")[0],
                Email = registerData.Email,
            };

            IdentityResult identityResult = await _userManager.CreateAsync(user, registerData.Password);

            if (!identityResult.Succeeded)
            {
                var error = string.Join(";", identityResult.Errors.Select(e => e.Description));
                return new AuthResultDto { Succeed = false, Error = error };
            }

            var token = _tokenService.GenerateAccessToken(user.Id, user.Email);

            return new AuthResultDto
            {
                Succeed = true,
                AccessToken = token.AccessToken,
                UserId = user.Id,
                Expiration = token.Expiration
            };
        }
    }
}