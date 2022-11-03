using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectName.Auth.Application.DTOs.Account;
using ProjectName.Auth.Application.Features.Login.Queries;
using ProjectName.Auth.Application.Features.Registration.Commands;
using ProjectName.Auth.Application.Wrappers;

namespace ProjectName.Auth.WebApi.Controllers.v1
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    public class AccountController : BaseController
    {
        public AccountController(IMediator mediator) : base(mediator)
        {
        }
        /// <summary>
        /// Вход пользователя в приложение
        /// </summary>        
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public async Task<IActionResult> Login([FromBody] LoginQuery query) => Result(await Mediator.Send(query));

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public async Task<IActionResult> Register([FromBody] RegistrationCommand cmd) => Result(await Mediator.Send(cmd));

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("reset-password")]
        [ProducesResponseType(typeof(Response), 200)]
        [ProducesResponseType(typeof(Response), 400)]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordRequest cmd) => Result(await Mediator.Send(cmd));
    }
}
