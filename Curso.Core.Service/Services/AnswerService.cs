using AutoMapper;
using Curso.Core.Data;
using Curso.Core.Data.Interfaces;
using Curso.Core.Model.DataModels;
using Curso.Core.Service.Interfaces;
using Curso.Core.Service.Requests;
using DevSnap.CommonLibrary.Pagination;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Curso.Core.Service.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly IARepository<Answer> _repository;
        private readonly CoreDbContext _context;
        private readonly IMapper _mapper;
        private const string NOT_FOUND = "Nenhum dado encontrado.";

        public AnswerService(IARepository<Answer> repository,
            CoreDbContext context,
            IMapper mapper)
        {
            _repository = repository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> GetAll(AnswerListRequestModel request)
        {
            try
            {
                var items = await Task.FromResult(_repository
                    .QueryableDetach());

                items = items.OrderByDescending(x => x.Id);

                if (request.PermitionAccessId.HasValue)
                    items = items.Where(x => x.PermitionAccessId == request.PermitionAccessId);

                return request.PageSize != 0
                    ? new OkObjectResult(Pagination<Answer>.Create(items, request.CurrentPage, request.PageSize))
                    : new OkObjectResult(items.ToList());

            }
            catch (Exception e)
            {
                ErrorLog.Log($"Houve uma falha ao buscar informações {e.Message} - {e.InnerException?.Message}");
                ErrorLog.Log($"{e.StackTrace}");
                return new BadRequestObjectResult(new { message = "Houve uma falha ao buscar as informações." });
            }
        }

        public async Task<IActionResult> GetOne(AnswerSingleRequestModel request)
        {
            try
            {
                var entity = await Task.FromResult(_repository.QueryableDetach()
                    .Where(x => x.Id == (int)request.Id));

                if (entity == null)
                    return new BadRequestObjectResult(new { message = NOT_FOUND });

                return new OkObjectResult(entity);
            }
            catch (Exception e)
            {
                ErrorLog.Log($"Houve uma falha ao buscar informações {e.Message} - {e.InnerException?.Message}");
                ErrorLog.Log($"{e.StackTrace}");
                return new BadRequestObjectResult(new { message = "Houve uma falha ao buscar as informações." });
            }
        }

        public async Task<IActionResult> Post(AnswerPostViewModel request)
        {
            try
            {
                if (request.PermitionAccessId != Guid.Empty)
                {
                    var entity = _repository
                    .QueryableDetach()
                    .Where(c => c.PermitionAccessId == request.PermitionAccessId).ToList();

                    foreach (var item in entity)
                    {
                        _repository.Delete(item);
                        _repository.Commit();
                    }
                }

                foreach (var item in request.Answers)
                {
                    _repository.Insert(new Answer
                    {
                        Question = item.Question,
                        Response = item.Response,
                        PermitionAccessId = request.PermitionAccessId

                    });
                }
                return await Task.FromResult(new OkObjectResult(request));
            }
            catch (Exception e)
            {
                ErrorLog.Log($"Houve uma falha ao salvar {e.Message} - {e.InnerException?.Message}");
                ErrorLog.Log($"{e.StackTrace}");
                return new BadRequestObjectResult(new { message = "Houve uma falha ao salvar o registro." });
            }
        }
        public async Task<IActionResult> Put(AnswerPutRequestModel request)
        {
            try
            {
                var entity = _repository
                    .QueryableDetach()
                    .FirstOrDefault(c => c.Id == request.Id);

                if (entity == null)
                    return new BadRequestObjectResult(new { message = NOT_FOUND });

                _repository.Update(request);
                _repository.Commit();

                return await Task.FromResult(new OkObjectResult(request));
            }
            catch (Exception e)
            {
                ErrorLog.Log($"Houve uma falha ao atualizar {e.Message} - {e.InnerException?.Message}");
                ErrorLog.Log($"{e.StackTrace}");
                return new BadRequestObjectResult(new { message = "Houve uma falha ao atualizar o registro." });
            }
        }

        public async Task<IActionResult> Delete(AnswerDeleteRequestModel request)
        {
            try
            {
                var entity = _repository.GetById(request.Id);
                if (entity == null)
                    return new BadRequestObjectResult(new { message = NOT_FOUND });

                _repository.Delete(request.Id);
                _repository.Commit();

                return await Task.FromResult(new OkObjectResult(new { message = "Resposta removida com sucesso." }));
            }
            catch (Exception e)
            {
                ErrorLog.Log($"Houve uma falha ao excluir {e.Message} - {e.InnerException?.Message}");
                ErrorLog.Log($"{e.StackTrace}");
                return new BadRequestObjectResult(new { message = "Houve uma falha ao excluir o registro." });
            }
        }

    }
}