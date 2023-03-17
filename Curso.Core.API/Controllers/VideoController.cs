using Curso.Core.Api.Controllers;
using Curso.Core.Service.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Curso.Core.API.Controllers
{
    //[Authorize]
    public class VideoController : ABaseController<VideoController,
        VideoListRequestModel,
        VideoSingleRequestModel,
        VideoDeleteRequestModel,
        VideoPostRequestModel,
        VideoPutRequestModel
        >
    {
        public VideoController(ILogger<VideoController> logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet("GetVideoDashboarding")]
        public virtual async Task<IActionResult> GetVideoDashboarding([FromQuery] VideoDashboardingRequestModel request)
        {
            return await _mediator.Send(request);
        }

        [HttpGet("GetActualVideoDashboarding")]
        public virtual async Task<IActionResult> GetActualVideoDashboarding([FromQuery] ActualVideoDashboardingRequestModel request)
        {
            return await _mediator.Send(request);
        }

        [HttpGet("GetNextVideoDashboarding")]
        public virtual async Task<IActionResult> GetNextVideoDashboarding([FromQuery] NextVideoDashboardingRequestModel request)
        {
            return await _mediator.Send(request);
        }
    }
}

