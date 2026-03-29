using DocuSense.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace DocuSense.Tests.Infrastructure
{
    public class KeycloakServiceTests
    {
        private Mock<IConfiguration> CreateMockConfiguration()
        {
            var mockConfig = new Mock<IConfiguration>();

            mockConfig.Setup(c => c["Keycloak:TokenEndpoint"])
                .Returns("http://localhost:8180/realms/docusense/protocol/openid-connect/token");
            mockConfig.Setup(c => c["Keycloak:ClientId"])
                .Returns("docusense-client");
            mockConfig.Setup(c => c["Keycloak:ClientSecret"])
                .Returns("test-secret");
            mockConfig.Setup(c => c["Keycloak:AdminEndpoint"])
                .Returns("http://localhost:8180/admin/realms/docusense");

            return mockConfig;
        }

        private HttpClient CreateMockHttpClient(HttpStatusCode statusCode, string responseContent)
        {
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(responseContent)
                });

            return new HttpClient(mockHandler.Object);
        }

        private HttpClient CreateSequentialMockHttpClient(HttpStatusCode firstStatusCode, string firstContent, HttpStatusCode secondStatusCode, string secondContent)
        {
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler
                .Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = firstStatusCode,
                    Content = new StringContent(firstContent)
                }).ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = secondStatusCode,
                    Content = new StringContent(secondContent)
                });

            return new HttpClient(mockHandler.Object);
        }

        [Fact]
        public async Task GetAdminToken_WhenSuccessful_ShouldReturnAccessToken()
        {
            var mockConfig = CreateMockConfiguration();

            var tokenJson = JsonSerializer.Serialize(new
            {
                access_token = "fake-admin-token-12345",
                expires_in = 300,
                token_type = "Bearer"
            });

            var httpClient = CreateMockHttpClient(HttpStatusCode.OK, tokenJson);
            var service = new KeycloakService(httpClient, mockConfig.Object);

            var result = await service.GetAdminTokenAsync();

            Assert.True(result.IsSuccess);
            Assert.Equal("fake-admin-token-12345", result.Data);
        }

        [Fact]
        public async Task GetAdminToken_WhenUnauthorized_ShouldReturnTokenAcquisitionFailed()
        {
            var mockConfig = CreateMockConfiguration();
            var httpClient = CreateMockHttpClient(HttpStatusCode.Unauthorized, "");
            var service = new KeycloakService(httpClient, mockConfig.Object);

            var result = await service.GetAdminTokenAsync();

            Assert.False(result.IsSuccess);
            Assert.Equal("Keycloak.TokenFailed", result.Error.Code);
        }

        [Fact]
        public async Task GetAdminToken_WhenBadRequest_ShouldReturnInvalidUserData()
        {
            var mockConfig = CreateMockConfiguration();
            var httpClient = CreateMockHttpClient(HttpStatusCode.BadRequest, "");
            var service = new KeycloakService(httpClient, mockConfig.Object);

            var result = await service.GetAdminTokenAsync();

            Assert.False(result.IsSuccess);
            Assert.Equal("Keycloak.InvalidUserData", result.Error.Code);
        }

        [Fact]
        public async Task GetAdminToken_WhenConfigMissing_ShouldReturnServiceUnavailable()
        {
            var mockConfig = new Mock<IConfiguration>();

            var httpClient = CreateMockHttpClient(HttpStatusCode.OK, "");
            var service = new KeycloakService(httpClient, mockConfig.Object);

            var result = await service.GetAdminTokenAsync();

            Assert.False(result.IsSuccess);
            Assert.Equal("Keycloak.Unavailable", result.Error.Code);
        }

        [Fact]
        public async Task GetAdminToken_WhenResponseHasNoAccessToken_ShouldReturnFailure()
        {
            var mockConfig = CreateMockConfiguration();

            var emptyTokenJson = JsonSerializer.Serialize(new
            {
                access_token = "",
                expires_in = 300
            });

            var httpClient = CreateMockHttpClient(HttpStatusCode.OK, emptyTokenJson);
            var service = new KeycloakService(httpClient, mockConfig.Object);

            var result = await service.GetAdminTokenAsync();

            Assert.False(result.IsSuccess);
            Assert.Equal("Keycloak.TokenFailed", result.Error.Code);
        }

        [Fact]
        public async Task CreateUser_WhenSuccessful_ShouldReturnSuccess()
        {
            var mockConfig = CreateMockConfiguration();

            var tokenJson = JsonSerializer.Serialize(new
            {
                access_token = "fake-admin-token",
                expires_in = 300
            });

            var httpClient = CreateSequentialMockHttpClient(HttpStatusCode.OK, tokenJson, HttpStatusCode.Created, "");

            var service = new KeycloakService(httpClient, mockConfig.Object);

            var result = await service.CreateUserAsync("test@test.com", "test123!", "Test", "User");

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task CreateUser_WhenUserAlreadyExist_ShouldReturnFailure()
        {
            var mockConfig = CreateMockConfiguration();

            var tokenJson = JsonSerializer.Serialize(new
            {
                access_token = "fake-admin-token",
                expires_in = 300
            });

            var httpClient = CreateSequentialMockHttpClient(HttpStatusCode.OK, tokenJson, HttpStatusCode.Conflict, "");

            var service = new KeycloakService(httpClient, mockConfig.Object);

            var result = await service.CreateUserAsync("test@test.com", "test123!", "Test", "User");

            Assert.False(result.IsSuccess);
            Assert.Equal("Keycloak.UserAlreadyExist", result.Error.Code);
        }

        [Fact]
        public async Task CreateUser_WhenInsufficientPermission_ShouldReturnFailure()
        {
            var mockConfig = CreateMockConfiguration();

            var tokenJson = JsonSerializer.Serialize(new
            {
                access_token = "fake-admin-token",
                expires_in = 300
            });

            var httpClient = CreateSequentialMockHttpClient(HttpStatusCode.OK, tokenJson, HttpStatusCode.Forbidden, "");

            var service = new KeycloakService(httpClient, mockConfig.Object);

            var result = await service.CreateUserAsync("test@test.com", "test123!", "Test", "User");

            Assert.False(result.IsSuccess);
            Assert.Equal("Keycloak.Forbidden", result.Error.Code);
        }
    }
}