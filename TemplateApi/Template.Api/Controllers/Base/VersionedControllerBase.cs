using Microsoft.AspNetCore.Mvc;

namespace Template.Api.Controllers.Base
{
    /// <summary>
    /// Base to develop all controller in the applciation
    /// </summary>
    [ApiController, Route("api/v{version:apiVersion}/[controller]")]
    public abstract class VersionedControllerBase : ControllerBase { }
}
