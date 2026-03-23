using DocuSense.Application.Services;
using DocuSense.Domain.Errors;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Text;
using System.Text.Json;
using static DocuSense.Domain.Errors.DomainErrors;

namespace DocuSense.Infrastructure.Services
{
    public class KeycloakService : IKeycloakService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public KeycloakService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<Result<bool>> CreateUserAsync(string email, string password, string firstName, string lastName)
        {
            var tokenResult = await GetAdminTokenAsync();

            if (!tokenResult.IsSuccess) return Result<bool>.Failure(KeycloakErrors.TokenAcquisitionFailed);

            var adminToken = tokenResult.Data;

            var adminEndpoint = _configuration["Keycloak:AdminEndpoint"];

            var userPayload = new
            {
                username = email,
                email = email,
                firstName = firstName,
                lastName = lastName,
                enabled = true,
                credentials = new[]
                {
                new
                    {
                    type = "password",
                    value = password,
                    temporary = false
                    }
                }
            };

            if (string.IsNullOrEmpty(adminToken)) Result<string>.Failure(DomainErrors.User.Unauthorized);

            //JSON body göndermek için. Kullanıcı oluştururken kullanılıyor.
            var jsonContent = new StringContent(JsonSerializer.Serialize(userPayload), Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminToken);

            var response = await _httpClient.PostAsync($"{adminEndpoint}/users", jsonContent);

            return response.StatusCode switch
            {
                HttpStatusCode.Created => Result<bool>.Success(true),
                HttpStatusCode.Conflict => Result<bool>.Failure(KeycloakErrors.UserAlreadyExist),
                HttpStatusCode.BadRequest => Result<bool>.Failure(KeycloakErrors.InvalidUserData),
                HttpStatusCode.Unauthorized => Result<bool>.Failure(KeycloakErrors.TokenAcquisitionFailed),
                HttpStatusCode.Forbidden => Result<bool>.Failure(KeycloakErrors.InsufficientPermissions),
                _ => Result<bool>.Failure(KeycloakErrors.ServiceUnavailable)
            };
        }

        public async Task<Result<string>> GetAdminTokenAsync()
        {
            var tokenEndpoint = _configuration["Keycloak:TokenEndpoint"];
            var clientId = _configuration["Keycloak:ClientId"];
            var clientSecret = _configuration["Keycloak:ClientSecret"];

            if (string.IsNullOrEmpty(tokenEndpoint) || string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
            {
                return Result<string>.Failure(DomainErrors.KeycloakErrors.ServiceUnavailable);
            }

            // x-www-form-urlencoded body oluşturma
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials",
                ["client_id"] = clientId,
                ["client_secret"] = clientSecret,
            });

            var response = await _httpClient.PostAsync(tokenEndpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                return response.StatusCode switch
                {
                    HttpStatusCode.Unauthorized =>
                    Result<string>.Failure(KeycloakErrors.TokenAcquisitionFailed),
                    HttpStatusCode.BadRequest =>
                        Result<string>.Failure(KeycloakErrors.InvalidUserData),
                    _ => Result<string>.Failure(KeycloakErrors.ServiceUnavailable)
                };
            }

            var json = await response.Content.ReadAsStringAsync();

            var tokenResponse = JsonSerializer.Deserialize<JsonElement>(json);

            var accessToken = tokenResponse.GetProperty("access_token").GetString();

            if (string.IsNullOrEmpty(accessToken))
            {
                return Result<string>.Failure(KeycloakErrors.TokenAcquisitionFailed);
            }

            return Result<string>.Success(accessToken);
        }
    }
}