using DocuSense.Application.Features.Commands.DeleteChat;
using DocuSense.Application.Features.Commands.UpdateChatTitle;
using DocuSense.Application.Features.Queries.Chat.GetAllChats;
using DocuSense.Application.Features.Queries.Commands.IngestData;
using DocuSense.Application.Features.Queries.Commands.Search;
using DocuSense.Application.Features.Queries.GetMessages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocuSense.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentController : BaseApiController
    {
        private readonly IMediator _mediator;

        public DocumentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("ingest")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Ingest(IFormFile? file)
        {
            var command = new IngestDataCommand(file);

            var result = await _mediator.Send(command);

            return HandleResult(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteChat([FromRoute] Guid id)
        {
            var command = new DeleteChatCommand(id);
            var result = await _mediator.Send(command);

            return HandleResult(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateChatTitle([FromBody] UpdateChatTitleCommand request)
        {
            var result = await _mediator.Send(request);

            return HandleResult(result);
        }

        [HttpGet("search")]
        public async Task Search([FromQuery] SearchCommand request, CancellationToken cancellationToken)
        {
            // 1. Next.js'in bu akışı okuyabilmesi için standart text ayarlıyoruz
            Response.ContentType = "text/plain; charset=utf-8";

            // 2. MediatR üzerinden şelaleyi (stream) başlatıyoruz
            var stream = _mediator.CreateStream(request, cancellationToken);

            // 3. Hortumdan gelen her bir damlayı (chunk) yakalıyoruz
            await foreach (var chunk in stream)
            {
                if (!chunk.IsSuccess)
                {
                    if (!Response.HasStarted)
                    {
                        Response.StatusCode = chunk.Error.Type switch
                        {
                            Domain.Errors.ErrorType.NotFound => 404,
                            Domain.Errors.ErrorType.Unauthorized => 401,
                            _ => 400
                        };
                        break;
                    }
                }

                if (chunk.Data != null)
                {
                    // Damlayı HTTP yanıtına (Response) yazıyoruz
                    await Response.WriteAsync(chunk.Data, cancellationToken);

                    // Kilit Nokta: Bekletmeden anında sifonu çekip Next.js'e fırlatıyoruz!
                    await Response.Body.FlushAsync(cancellationToken);
                }
                ;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllChats([FromQuery] GetAllChatsQuery request)
        {
            var result = await _mediator.Send(request);
            return HandleResult(result);
        }

        [HttpGet("{chatId}/messages")]
        public async Task<IActionResult> GetMessages([FromRoute] Guid chatId)
        {
            var request = new GetChatMessagesQuery(chatId);
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}