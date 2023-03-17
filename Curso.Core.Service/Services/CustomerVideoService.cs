using Curso.Core.Data;
using Curso.Core.Data.Interfaces;
using Curso.Core.Model.DataModels;
using DevSnap.CommonLibrary.Pagination;
using DevSnap.Core.Service.Interfaces;
using DevSnap.Core.Service.Requests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Curso.Core.Service.Services
{
    public class CustomerVideoService : ICustomerVideoService
    {
        private readonly IARepository<CustomerVideo> _repository;
        private readonly CoreDbContext _context;
        private const string NOT_FOUND = "Vídeo não encontrado";

        public CustomerVideoService(IARepository<CustomerVideo> repository, CoreDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<IActionResult> GetAll(CustomerVideoListRequestModel request)
        {
            try
            {
                var items = await Task.FromResult(_repository
                    .QueryableDetach()
                );

                if (request.PermitionAccessId != Guid.Empty)
                    items = items.Where(x => x.PermitionAccessId == request.PermitionAccessId);

                return request.PageSize != 0
                    ? new OkObjectResult(Pagination<CustomerVideo>.Create(items, request.CurrentPage, request.PageSize))
                    : new OkObjectResult(items.ToList());

            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(new { message = "Houve uma falha ao buscar" });
            }
        }

        public async Task<IActionResult> GetOne(CustomerVideoSingleRequestModel request)
        {
            try
            {
                var entity = await Task.FromResult(_repository.GetById(request.Id));
                if (entity == null)
                    return new BadRequestObjectResult(new { message = NOT_FOUND });

                return new OkObjectResult(entity);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(new { message = "Houve uma falha ao buscar." });
            }
        }

        public async Task<IActionResult> Post(CustomerVideoPostRequestModel request)
        {
            try
            {
                var entity = _repository
                    .QueryableDetach()
                    .FirstOrDefault(c => c.VideoId == request.VideoId && c.PermitionAccessId == request.PermitionAccessId);

                request.Video = null;

                if (entity == null)
                {
                    _repository.Insert(request);
                }
                else
                {
                    entity.IsViewed = request.IsViewed;
                    _repository.Update(entity);
                    _repository.Commit();
                }

                return await Task.FromResult(new OkObjectResult(request));
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(new { message = "Houve uma falha ao salvar" });
            }
        }

        public async Task<IActionResult> Put(CustomerVideoPutRequestModel request)
        {
            try
            {
                var entity = _repository
                    .QueryableDetach()
                    .FirstOrDefault(c => c.Id == request.Id);

                if (entity == null)
                    return new BadRequestObjectResult(new { message = NOT_FOUND });

                request.CreatedAt = entity.CreatedAt;

                _repository.Update(request);
                _repository.Commit();

                return await Task.FromResult(new OkObjectResult(request));
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(new { message = "Houve uma falha ao atualizar." });
            }
        }

        public async Task<IActionResult> Delete(CustomerVideoDeleteRequestModel request)
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
            catch (Exception e)
            {
                return new BadRequestObjectResult(new { message = "Houve uma falha ao excluir." });
            }
        }

        public List<CustomerVideo> GetCustomerVideos(Guid permitionAccessId, Guid? videoId)
        {
            var items = _repository.QueryableDetach();

            if (permitionAccessId != Guid.Empty)
                items = items.Where(x => x.PermitionAccessId == permitionAccessId);

            if (videoId != null && videoId != Guid.Empty)
                items = items.Where(x => x.PermitionAccessId == permitionAccessId && x.VideoId == videoId);

            return items.ToList();
        }
    }
}