using DocuSense.Application.Features.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DocuSense.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand request)
        {
            var result = await _mediator.Send(request);

            return HandleResult(result);
        }
    }
}