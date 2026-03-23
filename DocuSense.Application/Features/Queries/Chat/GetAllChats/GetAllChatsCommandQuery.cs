using DocuSense.Application.Data;
using DocuSense.Application.Dtos;
using DocuSense.Domain.Errors;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DocuSense.Application.Features.Queries.Chat.GetAllChats
{
    public class GetAllChatsCommandQuery : IRequestHandler<GetAllChatsQuery, Result<List<ChatListDto>>>
    {
        private readonly IDocuSenseAPIDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public GetAllChatsCommandQuery(IDocuSenseAPIDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public async Task<Result<List<ChatListDto>>> Handle(GetAllChatsQuery request, CancellationToken cancellationToken)
        {
            var userId = _contextAccessor.HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) return Result<List<ChatListDto>>.Failure(DomainErrors.User.Unauthorized);

            var chatListDto = await _context.Chats
                .AsNoTracking()
                .Where(c => c.UserId == userId)
                .Select(c => new ChatListDto
                {
                    CreatedAt = c.CreatedAt,
                    Id = c.Id,
                    DocumentId = c.VectorDocumentId,
                    Title = c.Title
                })
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();



            return Result<List<ChatListDto>>.Success(chatListDto);
        }
    }
}