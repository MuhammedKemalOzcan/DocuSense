using DocuSense.Application.Dtos.AuthenticationDto;

namespace DocuSense.Application.Services.Authentication
{
    public interface IIdentityService
    {
        Task<AuthResultDto> RegisterAsync(RegisterDto registerData);

        Task<AuthResultDto> LoginAsync(LoginDto loginData);
    }
}