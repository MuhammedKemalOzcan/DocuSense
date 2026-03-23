using DocuSense.Application.Dtos.AuthenticationDto;
using DocuSense.Domain.Errors;
using MediatR;

namespace DocuSense.Application.Features.Commands.Register
{
    public record RegisterUserCommand(RegisterDto Register) : IRequest<Result<bool>>;
}