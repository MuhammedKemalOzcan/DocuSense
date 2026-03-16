using DocuSense.Application.Dtos.AuthenticationDto;
using DocuSense.Application.Services.Authentication;
using MediatR;

namespace DocuSense.Application.Features.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthResultDto>
    {
        private readonly IIdentityService _identityService;

        public LoginUserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<AuthResultDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            AuthResultDto response = await _identityService.LoginAsync(new()
            {
                Email = request.Email,
                Password = request.Password
            });

            return response;
        }
    }
}