using Curso.Core.Service.Interfaces;
using Curso.Core.Service.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Curso.Core.Service.Handlers
{
    public class VideoRequestHandler :
        IRequestHandler<VideoListRequestModel, IActionResult>,
        IRequestHandler<VideoSingleRequestModel, IActionResult>,
        IRequestHandler<VideoDeleteRequestModel, IActionResult>,
        IRequestHandler<VideoPostRequestModel, IActionResult>,
        IRequestHandler<VideoPutRequestModel, IActionResult>,
        IRequestHandler<VideoDashboardingRequestModel, IActionResult>,
        IRequestHandler<NextVideoDashboardingRequestModel, IActionResult>,
        IRequestHandler<ActualVideoDashboardingRequestModel, IActionResult>
    {
        private readonly IVideoService _service;
        public VideoRequestHandler(IVideoService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Handle(VideoListRequestModel request, CancellationToken cancellationToken)
        {
            return await _service.GetAll(request);
        }

        public async Task<IActionResult> Handle(VideoSingleRequestModel request, CancellationToken cancellationToken)
        {
            return await _service.GetOne(request);
        }

        public async Task<IActionResult> Handle(VideoPostRequestModel request, CancellationToken cancellationToken)
        {

            return await _service.Post(request);
        }

        public async Task<IActionResult> Handle(VideoPutRequestModel request, CancellationToken cancellationToken)
        {
            return await _service.Put(request);
        }

        public async Task<IActionResult> Handle(VideoDeleteRequestModel request, CancellationToken cancellationToken)
        {
            return await _service.Delete(request);
        }
        public async Task<IActionResult> Handle(VideoDashboardingRequestModel request, CancellationToken cancellationToken)
        {
            return await _service.GetVideoDashboarding(request);
        }
        public async Task<IActionResult> Handle(ActualVideoDashboardingRequestModel request, CancellationToken cancellationToken)
        {
            return await _service.GetActualVideoDashboarding(request);
        }
        public async Task<IActionResult> Handle(NextVideoDashboardingRequestModel request, CancellationToken cancellationToken)
        {
            return await _service.GetNextVideoDashboarding(request);
        }

    }
}