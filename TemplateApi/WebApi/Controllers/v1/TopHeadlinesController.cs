using MediatR;
using Application;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.DTO.Base;

namespace WebApi.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = "ApiKey")]
    [Route("api/v{version:apiVersion}/[controller]"), ControllerName("top-headlines")]
    public class TopHeadlinesController(IMediator mediator, ILogger<SourceController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] TopHeadlinesEvent sourceEvent)
        {
            try
            {
                logger.LogInformation("requesting top-headlines...");

                ArticleResult result = await mediator.Send(sourceEvent);

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
