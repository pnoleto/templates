using Asp.Versioning;
using Template.Api.Controllers.Base;

namespace Template.Api.Controllers.v1.Base
{
    /// <summary>
    /// Base to develop all controller in the applciation
    /// </summary>
    [ApiVersion(1)]
    public abstract class ControllerV1 : VersionedControllerBase { }
}
