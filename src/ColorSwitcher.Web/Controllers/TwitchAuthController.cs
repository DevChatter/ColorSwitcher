using System.Threading.Tasks;
using ColorSwitcher.Core.TwitchAuth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ColorSwitcher.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwitchAuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TwitchAuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("This is a post-only API.");
        }

        [HttpPost]
        public async Task<IActionResult> Post(AccessTokenReceived accessTokenReceived)
        {
            var result = await _mediator.Send(accessTokenReceived);
            if (result)
            {
                return Ok();
            }

            return Problem("Failed to save auth token.");
        }
    }
}