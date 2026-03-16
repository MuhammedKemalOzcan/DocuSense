using DocuSense.Application.Dtos.AuthenticationDto;
using DocuSense.Application.Services.Authentication;
using MediatR;

namespace DocuSense.Application.Features.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResultDto>
    {
        private readonly IIdentityService _identityService;

        public RegisterUserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<AuthResultDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            AuthResultDto response = await _identityService.RegisterAsync(new()
            {
                Email = request.Email,
                Password = request.Password,
                FullName = request.Fullname,
            });

            return response;
        }
    }
}