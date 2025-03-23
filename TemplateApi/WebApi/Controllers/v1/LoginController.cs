using MediatR;
using Asp.Versioning;
using Application.Events;
using Application.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers.v1
{
    /// <summary>
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="logger"></param>
    /// <returns></returns>
    /// <response code="200">OK</response>
    /// <response code="401">Forbbiden Resource</response>
    /// <response code="500">Internal Server Error</response>
    [AllowAnonymous, ControllerName("Login")]
    public class LoginController(IMediator mediator, ILogger<SourceController> logger): BaseController
    {
        /// <summary>
        /// </summary>
        /// <param name="loginEvent"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] LoginEvent loginEvent)
        {
            logger.LogInformation("requesting login...");

            LoginResult result = await mediator.Send(loginEvent);

            return new OkObjectResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenEvent"></param>
        /// <returns></returns>
        [HttpPatch]
        public async Task<IActionResult> Patch([FromQuery] RenewAccessTokenEvent tokenEvent)
        {
            logger.LogInformation("requesting renew access token...");

            LoginResult result = await mediator.Send(tokenEvent);

            return new OkObjectResult(result);
        }
    }
}