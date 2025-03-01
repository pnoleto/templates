using MediatR;
using Application;
using Asp.Versioning;
using Application.DTO.Base;
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
    [ApiController,
     ApiVersion("1.0"),
     Authorize(AuthenticationSchemes = "ApiKey", Roles = "Admin"),
     Route("api/v{version:apiVersion}/[controller]"), ControllerName("sources")]
    public class SourceController(IMediator mediator, ILogger<SourceController> logger) : ControllerBase
    {
        /// <summary>
        /// Get Sources
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] SourceEvent source)
        {
            logger.LogInformation("requesting sources...");

            SourceResult result = await mediator.Send(source);

            return new OkObjectResult(result);
        }
    }
}
