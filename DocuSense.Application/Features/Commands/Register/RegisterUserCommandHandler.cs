using DocuSense.Application.Services;
using DocuSense.Domain.Errors;
using MediatR;

namespace DocuSense.Application.Features.Commands.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<bool>>
    {
        private readonly IKeycloakService _keycloakService;

        public RegisterUserCommandHandler(IKeycloakService keycloakService)
        {
            _keycloakService = keycloakService;
        }

        public async Task<Result<bool>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var isCreated = await _keycloakService.CreateUserAsync(request.Register.Email, request.Register.Password, request.Register.FirstName, request.Register.LastName);

            return isCreated;
        }
    }
}