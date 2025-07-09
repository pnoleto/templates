using Asp.Versioning;
using Template.Api.Controllers.Base;

namespace Template.Api.Controllers.v2.Base
{
    /// <summary>
    /// Base to develop all controller in the applciation
    /// </summary>
    [ApiVersion(2)]
    public abstract class ControllerV2 : VersionedControllerBase { }
}
