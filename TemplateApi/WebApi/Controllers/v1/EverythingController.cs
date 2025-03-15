using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Results;
using Application.Events;

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
    [ApiController,
     ApiVersion("1.0"),
     Authorize(AuthenticationSchemes = "ApiKey", Roles ="Admin"),
     Route("api/v{version:apiVersion}/[controller]"), ControllerName("everything")]
    public class EverythingController(IMediator mediator, ILogger<SourceController> logger) : ControllerBase
    {
        /// <summary>
        /// Get Everything
        /// </summary>
        /// <param name="article"></param>
        /// <returns>List of articles</returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] EverythingEvent article)
        {
            logger.LogInformation("requesting everything...");

            ArticleResult result = await mediator.Send(article);

            return new OkObjectResult(result);
        }
    }
}