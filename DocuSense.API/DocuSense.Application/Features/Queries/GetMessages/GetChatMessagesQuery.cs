using DocuSense.Application.Dtos;
using MediatR;

namespace DocuSense.Application.Features.Queries.GetMessages
{
    public record GetChatMessagesQuery(Guid ChatId) : IRequest<List<ChatMessagesListDto>>;
}