using DocuSense.Application.Data;
using DocuSense.Application.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocuSense.Application.Features.Queries.GetMessages
{
    public class GetChatMessagesQueryHandler : IRequestHandler<GetChatMessagesQuery, List<ChatMessagesListDto>>
    {
        private readonly IDocuSenseAPIDbContext _context;

        public GetChatMessagesQueryHandler(IDocuSenseAPIDbContext context)
        {
            _context = context;
        }

        public async Task<List<ChatMessagesListDto>> Handle(GetChatMessagesQuery request, CancellationToken cancellationToken)
        {
            var chatMessagesDto = await _context.ChatMessage
                .AsNoTracking()
                .Where(x => x.ChatId == request.ChatId)
                .OrderBy(x => x.CreatedAt)
                .Select(x => new ChatMessagesListDto
                {
                    Text = x.Text,
                    CreatedAt = x.CreatedAt,
                    IsUser = x.IsUser
                })
                .ToListAsync();

            return chatMessagesDto;
        }
    }
}