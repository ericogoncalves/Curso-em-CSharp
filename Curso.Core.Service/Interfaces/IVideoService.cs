using Curso.Core.Service.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Curso.Core.Service.Interfaces
{
    public interface IVideoService
    {
        public Task<IActionResult> GetAll(VideoListRequestModel request);
        Task<IActionResult> GetOne(VideoSingleRequestModel request);
        Task<IActionResult> Post(VideoPostRequestModel request);
        Task<IActionResult> Put(VideoPutRequestModel request);
        Task<IActionResult> Delete(VideoDeleteRequestModel request);
        Task<IActionResult> GetVideoDashboarding(VideoDashboardingRequestModel request);
        Task<IActionResult> GetActualVideoDashboarding(ActualVideoDashboardingRequestModel request);
        Task<IActionResult> GetNextVideoDashboarding(NextVideoDashboardingRequestModel request);
    }
}
