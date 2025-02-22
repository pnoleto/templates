using MediatR;
using Application;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.DTO.Base;

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
            try
            {
                logger.LogInformation("requesting sources...");

                SourceResult result = await mediator.Send(source);

                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                logger.LogError("Exception found.");

                return new BadRequestObjectResult(ex.Message);
            }
            finally
            {
                logger.LogInformation("Finished request.");
            }
        }
    }
}
