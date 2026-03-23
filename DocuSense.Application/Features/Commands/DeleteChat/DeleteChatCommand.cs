using DocuSense.Domain.Entites;
using DocuSense.Domain.Errors;
using MediatR;

namespace DocuSense.Application.Features.Commands.DeleteChat
{
    public record DeleteChatCommand(Guid Id) : IRequest<Result<Guid>>;
}