using DocuSense.Application.Dtos.AuthenticationDto;
using MediatR;

namespace DocuSense.Application.Features.Commands.RegisterUser
{
    public record RegisterUserCommand(string Fullname, string Email, string Password) : IRequest<AuthResultDto>;
}