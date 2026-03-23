using DocuSense.Application.Dtos;
using DocuSense.Domain.Errors;
using MediatR;

namespace DocuSense.Application.Features.Queries.Chat.GetAllChats
{
    public record GetAllChatsQuery() : IRequest<Result<List<ChatListDto>>>;
}
