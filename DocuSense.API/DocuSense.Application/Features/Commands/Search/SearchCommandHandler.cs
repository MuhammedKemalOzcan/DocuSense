using DocuSense.Application.Data;
using DocuSense.Application.Dtos;
using DocuSense.Application.Services;
using DocuSense.Domain.Entites;
using DocuSense.Domain.Errors;
using DocuSense.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;

namespace DocuSense.Application.Features.Queries.Commands.Search
{
    public class SearchCommandHandler : IStreamRequestHandler<SearchCommand, Result<string>>
    {
        private readonly IVectorDatabaseService _vectorDatabaseService;
        private readonly IChatGenerationService _chatGenerationService;
        private readonly IChatMessageRepository _chatMessageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDocuSenseAPIDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public SearchCommandHandler(IVectorDatabaseService vectorDatabaseService, IChatGenerationService chatGenerationService, IChatMessageRepository chatMessageRepository, IUnitOfWork unitOfWork, IDocuSenseAPIDbContext context, IHttpContextAccessor contextAccessor)
        {
            _vectorDatabaseService = vectorDatabaseService;
            _chatGenerationService = chatGenerationService;
            _chatMessageRepository = chatMessageRepository;
            _unitOfWork = unitOfWork;
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public async IAsyncEnumerable<Result<string>> Handle(SearchCommand request, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var userId = _contextAccessor.HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) 
            {
                yield return Result<string>.Failure(DomainErrors.User.Unauthorized);
                yield break;
            } 

            var userMessage = new ChatMessage
            {
                Id = Guid.NewGuid(),
                ChatId = Guid.Parse(request.ChatId),
                Text = request.Query,
                CreatedAt = DateTime.UtcNow,
                IsUser = true
            };

            _chatMessageRepository.Add(userMessage);

            //agent'ın konuşmanın bağlamını anlayabilmesi için chatHistory'den son 5 mesajı çekiyoruz ve bunu arama (searchDataAsync) metodumuza veriyoruz. Prompt içerisinde bu bilgileri ajana vereceğiz.
            var chatIdGuid = Guid.Parse(request.ChatId);

            var chatHistory = await _context.ChatMessage
                .AsNoTracking()
                .Where(x => x.ChatId == chatIdGuid)
                .OrderByDescending(x => x.CreatedAt)
                .Take(5)
                .OrderBy(x => x.CreatedAt)
                .Select(x => new ChatMessagesListDto
                {
                    Text = x.Text,
                    CreatedAt = x.CreatedAt,
                    IsUser = x.IsUser
                })
                .ToListAsync();

            var chat = await _context.Chats
                .AsNoTracking()
                .Where(x => x.Id == chatIdGuid)
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();

            if (chat == null)
            {
                yield return Result<string>.Failure(DomainErrors.Chat.NotFound);
                yield break;
            }

            var datas = await _vectorDatabaseService.SearchDataAsync(request.Query, request.Top, chat.VectorDocumentId);

            StringBuilder stringBuilder = new StringBuilder();

            var chatResult = _chatGenerationService.GetChatMessageAsync(datas, request.Query, chatHistory);

            await foreach (var item in chatResult)
            {
                stringBuilder.Append(item);
                yield return Result<string>.Success(item);
            }

            var aiMessage = new ChatMessage
            {
                Id = Guid.NewGuid(),
                ChatId = Guid.Parse(request.ChatId),
                Text = stringBuilder.ToString(),
                CreatedAt = DateTime.UtcNow,
                IsUser = false
            };

            _chatMessageRepository.Add(aiMessage);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}