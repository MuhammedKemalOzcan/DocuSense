using DocuSense.Application.Dtos;
using DocuSense.Application.Services;
using DocuSense.Domain.Entites;
using DocuSense.Domain.Repositories;
using DocuSense.Persistence.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DocuSense.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IPdfReaderService _pdfReaderService;
        private readonly ITextChunkerService _textChunkerService;
        private readonly IVectorDatabaseService _vectorDatabaseService;
        private readonly IChatGenerationService _chatGeneration;
        private readonly IChatRepository _chatRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly DocuSenseAPIDbContext _context;

        public DocumentController(IPdfReaderService pdfReaderService, ITextChunkerService textChunkerService, IVectorDatabaseService vectorDatabaseService, IChatGenerationService chatGeneration, IChatRepository chatRepository, IUnitOfWork unitOfWork, DocuSenseAPIDbContext context)
        {
            _pdfReaderService = pdfReaderService;
            _textChunkerService = textChunkerService;
            _vectorDatabaseService = vectorDatabaseService;
            _chatGeneration = chatGeneration;
            _chatRepository = chatRepository;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        [HttpPost("extract-text")]
        public async Task<IActionResult> UploadPdf(string filePath)
        {
            var result = _pdfReaderService.ExtractTextFromPdf(filePath);
            return Ok(result);
        }

        [HttpPost("ingest")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Ingest(IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest("Geçerli Bir Dosya Yüklenmedi!");
            var filePath = Path.GetTempFileName();
            var documentId = Guid.NewGuid().ToString();

            var newChat = new Chat
            {
                Id = Guid.NewGuid(),
                VectorDocumentId = documentId,
                Title = file.FileName,
                CreatedAt = DateTime.UtcNow,
            };

            _chatRepository.Add(newChat);

            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
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

            return Ok(new IngestResultDto
            {
                ChatId = newChat.Id,
                ChatTitle = newChat.Title,
                DocumentId = documentId
            });
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query, int top, string documentId)
        {
            var datas = await _vectorDatabaseService.SearchDataAsync(query, top, documentId);

            var chatResult = await _chatGeneration.GetChatMessageAsync(datas, query);

            return Ok(chatResult);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllChats()
        {
            var chats = await _context.Chats
                .AsNoTracking()
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return Ok(chats);
        }
    }
}