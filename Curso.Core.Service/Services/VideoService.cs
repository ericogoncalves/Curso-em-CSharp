using AutoMapper;
using Curso.Core.Data.Interfaces;
using Curso.Core.Model.DataModels;
using Curso.Core.Service.Interfaces;
using Curso.Core.Service.Requests;
using DevSnap.CommonLibrary.Pagination;
using DevSnap.Core.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Curso.Core.Service.Services
{
    public class VideoService : IVideoService
    {
        private readonly IARepository<Video> _repository;
        private readonly IMapper _mapper;
        private readonly ICustomerVideoService _customerVideo;

        public VideoService(IARepository<Video> repository, ICustomerVideoService _customerVideoService, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _customerVideo = _customerVideoService;
        }
        private const string NOT_FOUND = "Item não encontrado.";


        public async Task<IActionResult> GetAll(VideoListRequestModel request)
        {
            try
            {
                var items = await Task.FromResult(_repository.Queryable());

                if (!string.IsNullOrEmpty(request.Description))
                    items = items.Where(x => x.Description.Contains(request.Description));

                if (!string.IsNullOrEmpty(request.Search))
                    items = items.Where(x => x.Description.ToLower().Contains(request.Search.ToLower()));

                return new OkObjectResult(Pagination<Video>.Create(items, request.CurrentPage, request.PageSize));
            }
            catch (Exception ex)
            {
                ErrorLog.Log($"Erro ao realizar a ação {ex.Message} - {ex.InnerException?.Message}");
                ErrorLog.Log(ex.StackTrace);
                return new BadRequestObjectResult(new { message = "Houve uma falha ao buscar as informações." });
            }
        }

        public async Task<IActionResult> GetOne(VideoSingleRequestModel request)
        {
            try
            {
                var entity = await Task.FromResult(_repository.GetById(request.Id));
                if (entity == null)
                    return new BadRequestObjectResult(new { message = NOT_FOUND });

                return new OkObjectResult(entity);
            }
            catch (Exception ex)
            {
                ErrorLog.Log($"Erro ao realizar a ação {ex.Message} - {ex.InnerException?.Message}");
                ErrorLog.Log(ex.StackTrace);
                return new BadRequestObjectResult(new { message = "Houve uma falha ao buscar as informações." });
            }
        }

        public async Task<IActionResult> Post(VideoPostRequestModel request)
        {
            try
            {
                var obj = _repository.QueryableDetach()
                    .FirstOrDefault();

                _repository.Insert(request);
                return await Task.FromResult(new OkObjectResult(request));
            }
            catch (Exception ex)
            {
                ErrorLog.Log($"Erro ao realizar a ação {ex.Message} - {ex.InnerException?.Message}");
                ErrorLog.Log(ex.StackTrace);
                return new BadRequestObjectResult(new { message = "Houve uma falha ao salvar o vídeo." });
            }
        }

        public async Task<IActionResult> Put(VideoPutRequestModel request)
        {
            try
            {
                var entity = _repository.QueryableDetach()
                                       .Where(c => c.Id == request.Id)
                                       .FirstOrDefault();
                if (entity == null)
                    return new BadRequestObjectResult(new { message = NOT_FOUND });


                _repository.Update(request);
                _repository.Commit();

                return await Task.FromResult(new OkObjectResult(request));
            }
            catch (Exception ex)
            {
                ErrorLog.Log($"Erro ao realizar a ação {ex.Message} - {ex.InnerException?.Message}");
                ErrorLog.Log(ex.StackTrace);
                return new BadRequestObjectResult(new { message = "Houve uma falha ao atualizar o vídeo." });
            }
        }

        public async Task<IActionResult> Delete(VideoDeleteRequestModel request)
        {
            try
            {
                var entity = _repository.GetById(request.Id);
                if (entity == null)
                    return new BadRequestObjectResult(new { message = NOT_FOUND });

                _repository.Delete(request.Id);
                _repository.Commit();

                return await Task.FromResult(new OkResult());
            }
            catch (Exception ex)
            {
                ErrorLog.Log($"Erro ao realizar a ação {ex.Message} - {ex.InnerException?.Message}");
                ErrorLog.Log(ex.StackTrace);
                return new BadRequestObjectResult(new { message = "Houve uma falha ao excluir o vídeo." });
            }

        }
        public async Task<IActionResult> GetVideoDashboarding(VideoDashboardingRequestModel request)
        {

            var items = await Task.FromResult(
                _repository.Queryable()
                .OrderBy(v => v.Order).ToList());

            if (request.PermitionAccessId != null || request.PermitionAccessId == Guid.Empty)
            {
                //marcar o vídeo como lido
                var lstVideosViewds = _customerVideo.GetCustomerVideos((Guid)request.PermitionAccessId);

                foreach (var item in items)
                {
                    var videoViewed = lstVideosViewds.Where(x => x.VideoId == item.Id).FirstOrDefault();
                    if (videoViewed != null)
                        item.IsViewed = videoViewed.IsViewed;
                }
            }

            return new OkObjectResult(items.ToList());
        }

        public async Task<IActionResult> GetActualVideoDashboarding(ActualVideoDashboardingRequestModel request)
        {

            var items = await Task.FromResult(
                _repository.Queryable()
                .OrderBy(v => v.Order).ToList());

            if (request.PermitionAccessId != null || request.PermitionAccessId == Guid.Empty)
            {
                //marcar o vídeo como lido
                var lstVideosViewds = _customerVideo.GetCustomerVideos((Guid)request.PermitionAccessId);

                foreach (var item in items)
                {
                    var videoViewed = lstVideosViewds.Where(x => x.VideoId == item.Id).FirstOrDefault();
                    if (videoViewed != null)
                        item.IsViewed = videoViewed.IsViewed;
                }
            }

            return new OkObjectResult(items.Where(x => !x.IsViewed).FirstOrDefault());
        }

        public async Task<IActionResult> GetNextVideoDashboarding(NextVideoDashboardingRequestModel request)
        {

            var items = await Task.FromResult(
                _repository.Queryable()
                .OrderBy(v => v.Order).ToList());

            if (request.PermitionAccessId != null || request.PermitionAccessId == Guid.Empty)
            {
                //marcar o vídeo como lido
                var lstVideosViewds = _customerVideo.GetCustomerVideos((Guid)request.PermitionAccessId);

                foreach (var item in items)
                {
                    var videoViewed = lstVideosViewds.Where(x => x.VideoId == item.Id).FirstOrDefault();
                    if (videoViewed != null)
                        item.IsViewed = videoViewed.IsViewed;
                }
            }

            items = items.Where(x => !x.IsViewed).ToList();
            items.Remove(items.FirstOrDefault());

            return new OkObjectResult(items.FirstOrDefault());
        }

    }
}
