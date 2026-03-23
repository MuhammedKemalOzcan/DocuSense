using DocuSense.Domain.Errors;

namespace DocuSense.Application.Services
{
    public interface IKeycloakService
    {
        Task<Result<string>> GetAdminTokenAsync();

        Task<Result<bool>> CreateUserAsync(string email, string password, string firstName, string lastName);
    }
}