using Asp.Versioning;
using Application.DTO;
using Application.Results;
using Microsoft.AspNetCore.Mvc;
using Application.Mediator.Interface;
using Template.Api.Controllers.v2.Base;

namespace Template.Api.Controllers.v2;

/// <summary>
/// 
/// </summary>
/// <param name="mediator"></param>
[ControllerName("home")]
public class HomeController(IMediator mediator) : ControllerV2
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet, Route(""), EndpointName("v2_home_GetAsync")]
    public async Task<ActionResult> GetAsync()
    {
        return Ok(await mediator.Send<Test1, Result>(new Test1(), new CancellationToken()));
    }

}
