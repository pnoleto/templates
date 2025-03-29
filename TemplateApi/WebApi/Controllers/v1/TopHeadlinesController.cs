using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
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
    [ControllerName("top-headlines")]
    public class TopHeadlinesController(IMediator mediator, ILogger<SourceController> logger) : ApiController
    {
        /// <summary>
        /// Get Top headlines
        /// </summary>
        /// <param name="sourceEvent"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] TopHeadlinesEvent sourceEvent)
        {
            logger.LogInformation("requesting top-headlines...");

            ArticleResult result = await mediator.Send(sourceEvent);

            return new OkObjectResult(result);
        }
    }
}
