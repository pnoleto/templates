using Application.DTO;
using Application.Mediator.Interface;
using Application.Results;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Template.Api.Controllers.v1;

/// <summary>
/// 
/// </summary>
/// <param name="mediator"></param>
/// <param name="queryMediator"></param>
[ControllerName("home")]
public class HomeController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet, Route("")]
    public async Task<ActionResult> GetAsync()
    {
        return Ok(mediator.Send<Test1, Result>(new Test1(), new CancellationToken()));
    }

}
