using DocuSense.Domain.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocuSense.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected IActionResult HandleResult<T>(Result<T> result)
        {
            if (result.IsSuccess)
            {
                if (result.Data == null) return NoContent();

                return Ok(result.Data);
            }

            return result.Error.Type switch
            {
                ErrorType.Validation => BadRequest(result.Error),
                ErrorType.Unauthorized => Unauthorized(result.Error),
                ErrorType.NotFound => NotFound(result.Error),
                ErrorType.Failure => BadRequest(result.Error),
                _ => BadRequest(result.Error)
            };
        }
    }
}