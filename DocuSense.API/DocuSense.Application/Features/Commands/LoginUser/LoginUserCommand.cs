using DocuSense.Application.Dtos.AuthenticationDto;
using MediatR;

namespace DocuSense.Application.Features.Commands.LoginUser
{
    public record LoginUserCommand(string Email, string Password) : IRequest<AuthResultDto>;
}