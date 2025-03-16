using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1
{
    /// <summary>
    /// Base to develop all controller in the applciation
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="logger"></param>
    [Authorize,
     ApiController,
     ApiVersion("1.0"),
     Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseController(IMediator mediator, ILogger<SourceController> logger) : Controller
    {
    }
}
