using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Shared.Mediator;
using Shared.Mediator.Interface;
using Template.Api.Controllers.v2.Base;

namespace Template.Api.Controllers.v1
{
    [ControllerName("home")]
    public class HomeController : ControllerV2
    {
        private readonly IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: HomeController
        [HttpGet, Route("")]
        public ActionResult Index()
        {
            _mediator.Send(new Test(), new CancellationToken());
            
            return Ok();
        }

    }
}
