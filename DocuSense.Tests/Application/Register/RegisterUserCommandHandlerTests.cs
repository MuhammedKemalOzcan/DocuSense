using DocuSense.Application.Features.Commands.Register;
using DocuSense.Application.Services;
using DocuSense.Domain.Errors;
using Moq;
using static DocuSense.Domain.Errors.DomainErrors;

namespace DocuSense.Tests.Application.Register
{
    public class RegisterUserCommandHandlerTests
    {
        [Fact]
        public async Task Handle_WhenServiceReturnsSuccess_ShouldReturnSuccess()
        {
            var mockService = new Mock<IKeycloakService>();

            mockService.Setup(s => s.CreateUserAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
                .ReturnsAsync(Result<bool>.Success(true));

            var handler = new RegisterUserCommandHandler(mockService.Object);

            var command = new RegisterUserCommand(new DocuSense.Application.Dtos.AuthenticationDto.RegisterDto
            {
                Email = "test@test.com",
                Password = "Test1234!",
                FirstName = "Test",
                LastName = "User"
            });

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_WhenUserAlreadyExist_ShouldReturnFailure()
        {
            var mockService = new Mock<IKeycloakService>();

            mockService.Setup(s => s.CreateUserAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
                .ReturnsAsync(Result<bool>.Failure(KeycloakErrors.UserAlreadyExist));

            var handler = new RegisterUserCommandHandler(mockService.Object);

            var command = new RegisterUserCommand(new DocuSense.Application.Dtos.AuthenticationDto.RegisterDto
            {
                Email = "test@test.com",
                Password = "Test1234!",
                FirstName = "Test",
                LastName = "User"
            });

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("Keycloak.UserAlreadyExist", result.Error.Code);
        }

        [Fact]
        public async Task Handle_WhenKeycloakUnavailable_ShouldReturnFailure()
        {
            var mockService = new Mock<IKeycloakService>();

            mockService.Setup(s => s.CreateUserAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
                .ReturnsAsync(Result<bool>.Failure(KeycloakErrors.ServiceUnavailable));

            var handler = new RegisterUserCommandHandler(mockService.Object);

            var command = new RegisterUserCommand(new DocuSense.Application.Dtos.AuthenticationDto.RegisterDto
            {
                Email = "test@test.com",
                Password = "Test1234!",
                FirstName = "Test",
                LastName = "User"
            });

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("Keycloak.Unavailable", result.Error.Code);
        }
    }
}