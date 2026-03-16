using DocuSense.Application.Dtos.AuthenticationDto;

namespace DocuSense.Application.Services.Authentication
{
    public interface ITokenService
    {
        TokenDto GenerateAccessToken(string userId, string email);
    }
}