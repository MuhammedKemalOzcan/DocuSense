using DocuSense.Domain.Errors;
using DocuSense.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DocuSense.Application.Features.Commands.DeleteChat
{
    public class DeleteChatCommandHandler : IRequestHandler<DeleteChatCommand, Result<Guid>>
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;

        public DeleteChatCommandHandler(IChatRepository chatRepository, IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _chatRepository = chatRepository;
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
        }

        public async Task<Result<Guid>> Handle(DeleteChatCommand request, CancellationToken cancellationToken)
        {
            var userId = _contextAccessor.HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) return Result<Guid>.Failure(DomainErrors.Chat.Unauthorized);

            var chat = await _chatRepository.GetByIdAsync(request.Id, userId);

            if (chat == null) return Result<Guid>.Failure(DomainErrors.Chat.NotFound);

            _chatRepository.Remove(chat);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(chat.Id);
        }
    }
}