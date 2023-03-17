using Curso.Core.Model.DataModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Curso.Core.Service.Requests
{
    public class VideoListRequestModel : ABaseListRequestModel
    {
        public int Order { get; set; }
        public string Description { get; set; }
    }
    public class VideoSingleRequestModel : ABaseSingleRequestModel { }
    public class VideoPostRequestModel : Video, IRequest<IActionResult> { }
    public class VideoPutRequestModel : Video, IRequest<IActionResult> { }
    public class VideoDeleteRequestModel : ABaseDeleteRequestModel { }
    public class VideoResponseModel : Video, IRequest<IActionResult> { }
    public class VideoDashboardingRequestModel : IRequest<IActionResult>
    {
        public Guid? PermitionAccessId { get; set; }
    }
    public class ActualVideoDashboardingRequestModel : IRequest<IActionResult>
    {
        public Guid? PermitionAccessId { get; set; }
    }
    public class NextVideoDashboardingRequestModel : IRequest<IActionResult>
    {
        public Guid? PermitionAccessId { get; set; }
    }
}