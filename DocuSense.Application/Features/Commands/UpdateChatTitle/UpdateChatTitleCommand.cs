using DocuSense.Application.Dtos;
using DocuSense.Domain.Entites;
using DocuSense.Domain.Errors;
using MediatR;

namespace DocuSense.Application.Features.Commands.UpdateChatTitle
{
    public record UpdateChatTitleCommand(Guid Id,string Title) : IRequest<Result<ChatListDto>>;
}