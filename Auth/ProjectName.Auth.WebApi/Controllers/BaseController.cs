using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectName.Auth.Application.Enums;
using ProjectName.Auth.Application.Wrappers;

namespace ProjectName.Auth.WebApiz.Controllers
{
    [ApiController]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [Route("api/v{version:apiVersion}/")]
    public abstract class BaseController : ControllerBase
    {
        private protected readonly IMediator Mediator;

        protected BaseController(IMediator mediator)
        {
            Mediator = mediator;
        }

        private protected IActionResult Result(Response response)
        {
            return response.Status switch
            {
                ResponseStatus.Success => Ok(response),
                ResponseStatus.Error => BadRequest(response),
                ResponseStatus.NotFound => NotFound(response),
                ResponseStatus.Forbidden => Forbid(),
                _ => throw new ArgumentOutOfRangeException(
                    $@"The value needs to be one of {string.Join(", ", Enum.GetNames(typeof(ResponseStatus)))}.")
            };
        }
    }
}