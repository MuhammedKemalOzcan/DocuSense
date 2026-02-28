using DocuSense.Application.Services;
using Microsoft.AspNetCore.Mvc;

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

        public DocumentController(IPdfReaderService pdfReaderService, ITextChunkerService textChunkerService, IVectorDatabaseService vectorDatabaseService, IChatGenerationService chatGeneration)
        {
            _pdfReaderService = pdfReaderService;
            _textChunkerService = textChunkerService;
            _vectorDatabaseService = vectorDatabaseService;
            _chatGeneration = chatGeneration;
        }

        [HttpPost("extract-text")]
        public async Task<IActionResult> UploadPdf(string filePath)
        {
            var result = _pdfReaderService.ExtractTextFromPdf(filePath);
            return Ok(result);
        }

        [HttpPost("ingest")]
        public async Task<IActionResult> Ingest(string filePath)
        {
            var text = _pdfReaderService.ExtractTextFromPdf(filePath);
            var chunkedText = _textChunkerService.ChunkText(text, 500);
            await _vectorDatabaseService.IngestDataAsync(chunkedText);

            return Ok("Veriler başarılı bir şekilde vektör uzaya eklendi!");
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search(string query, int top)
        {
            var datas = await _vectorDatabaseService.SearchDataAsync(query, top);

            var chatResult = await _chatGeneration.GetChatMessageAsync(datas, query);

            return Ok(chatResult);
        }
    }
}