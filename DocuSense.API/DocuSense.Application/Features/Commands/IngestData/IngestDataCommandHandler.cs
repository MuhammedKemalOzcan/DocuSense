using DocuSense.Application.Dtos;
using DocuSense.Application.Services;
using DocuSense.Domain.Errors;
using DocuSense.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DocuSense.Application.Features.Queries.Commands.IngestData
{
    public class IngestDataCommandHandler : IRequestHandler<IngestDataCommand, Result<IngestResultDto>>
    {
        private readonly IChatRepository _chatRepository;
        private readonly IPdfReaderService _pdfReaderService;
        private readonly ITextChunkerService _textChunkerService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVectorDatabaseService _vectorDatabaseService;
        private readonly IHttpContextAccessor _contextAccessor;

        public IngestDataCommandHandler(IChatRepository chatRepository, IPdfReaderService pdfReaderService, ITextChunkerService textChunkerService, IUnitOfWork unitOfWork, IVectorDatabaseService vectorDatabaseService, IHttpContextAccessor contextAccessor)
        {
            _chatRepository = chatRepository;
            _pdfReaderService = pdfReaderService;
            _textChunkerService = textChunkerService;
            _unitOfWork = unitOfWork;
            _vectorDatabaseService = vectorDatabaseService;
            _contextAccessor = contextAccessor;
        }

        public async Task<Result<IngestResultDto>> Handle(IngestDataCommand request, CancellationToken cancellationToken)
        {
            var userId = _contextAccessor.HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) return Result<IngestResultDto>.Failure(DomainErrors.User.Unauthorized);

            if (request.File == null || request.File.Length == 0) return Result<IngestResultDto>.Failure(DomainErrors.File.NotFound);
            var filePath = Path.GetTempFileName();
            var documentId = Guid.NewGuid().ToString();

            var newChat = new Domain.Entites.Chat
            {
                Id = Guid.NewGuid(),
                VectorDocumentId = documentId,
                Title = request.File.FileName,
                CreatedAt = DateTime.UtcNow,
                UserId = userId,
            };

            _chatRepository.Add(newChat);

            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.File.CopyToAsync(fileStream);
                    await fileStream.FlushAsync();
                }
                ;
                var text = _pdfReaderService.ExtractTextFromPdf(filePath);
                var chunkedText = _textChunkerService.ChunkText(text, 500);
                await _vectorDatabaseService.IngestDataAsync(chunkedText, documentId);
                await _unitOfWork.SaveChangesAsync(CancellationToken.None);
            }
            finally
            {
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            var result = new IngestResultDto
            {
                Id = newChat.Id,
                Title = newChat.Title,
                DocumentId = documentId,
                CreatedAt = DateTime.UtcNow
            };

            return Result<IngestResultDto>.Success(result);
        }
    }
}