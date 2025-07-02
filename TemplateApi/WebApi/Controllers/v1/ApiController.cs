using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1
{
    /// <summary>
    /// Base to develop all controller in the applciation
    /// </summary>
    [Authorize(AuthenticationSchemes = "Bearer"),
     ApiController,
     ApiVersion("1.0"),
     Route("api/v{version:apiVersion}/[controller]")]
    public abstract class ApiController : ControllerBase { }
}
