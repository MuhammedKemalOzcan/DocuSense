using DocuSense.Application.Dtos.AuthenticationDto;
using DocuSense.Application.Features.Commands.LoginUser;
using DocuSense.Application.Features.Commands.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DocuSense.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterUserCommand request)
        {
            var response = await _mediator.Send(request);

            if (!response.Succeed) return BadRequest(response.Error);

            return Ok(new TokenDto { AccessToken = response.AccessToken, Expiration = response.Expiration });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand request)
        {
            var response = await _mediator.Send(request);

            if (!response.Succeed) return BadRequest(response.Error);

            return Ok(new TokenDto { AccessToken = response.AccessToken, Expiration = response.Expiration });
        }
    }
}