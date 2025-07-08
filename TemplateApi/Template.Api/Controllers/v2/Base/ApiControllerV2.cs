using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Template.Api.Controllers.v2.Base
{
    /// <summary>
    /// Base to develop all controller in the applciation
    /// </summary>
    [ApiVersion(2)]
    public abstract class ControllerV2 : ControllerBase { }
}
