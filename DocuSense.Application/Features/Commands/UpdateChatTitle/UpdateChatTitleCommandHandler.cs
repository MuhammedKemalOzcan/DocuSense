using DocuSense.Application.Dtos;
using DocuSense.Domain.Entites;
using DocuSense.Domain.Errors;
using DocuSense.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DocuSense.Application.Features.Commands.UpdateChatTitle
{
    public class UpdateChatTitleCommandHandler : IRequestHandler<UpdateChatTitleCommand, Result<ChatListDto>>
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;

        public UpdateChatTitleCommandHandler(IChatRepository chatRepository, IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _chatRepository = chatRepository;
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
        }

        public async Task<Result<ChatListDto>> Handle(UpdateChatTitleCommand request, CancellationToken cancellationToken)
        {
            var userId = _contextAccessor.HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) return Result<ChatListDto>.Failure(DomainErrors.Chat.Unauthorized);

            var chat = await _chatRepository.GetByIdAsync(request.Id, userId);

            if (chat == null) return Result<ChatListDto>.Failure(DomainErrors.Chat.NotFound);

            chat.Title = request.Title;

            _chatRepository.Update(chat);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var chatDto = new ChatListDto
            {
                Id = chat.Id,
                CreatedAt = chat.CreatedAt,
                DocumentId = chat.VectorDocumentId,
                Title = chat.Title
            };

            return Result<ChatListDto>.Success(chatDto);
        }
    }
}