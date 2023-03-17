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
    public class QuestionService : IQuestionService
    {
        private readonly IARepository<Question> _repository;
        private readonly CoreDbContext _context;
        private readonly IMapper _mapper;
        private const string NOT_FOUND = "Nenhum dado encontrado.";

        public QuestionService(IARepository<Question> repository,
            CoreDbContext context,
            IMapper mapper)
        {
            _repository = repository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> GetAll(QuestionListRequestModel request)
        {
            try
            {
                var items = await Task.FromResult(_repository
                    .QueryableDetach()
                );

                if (!string.IsNullOrEmpty(request.Description))
                    items = items.Where(x => x.Description.ToLower().Contains(request.Description.ToLower()));

                

                items = items.OrderByDescending(x => x.Id);

                return request.PageSize != 0
                    ? new OkObjectResult(Pagination<Question>.Create(items, request.CurrentPage, request.PageSize))
                    : new OkObjectResult(items.ToList());

            }
            catch (Exception e)
            {
                ErrorLog.Log($"Houve uma falha ao buscar informações {e.Message} - {e.InnerException?.Message}");
                ErrorLog.Log($"{e.StackTrace}");
                return new BadRequestObjectResult(new { message = "Houve uma falha ao buscar as informações." });
            }
        }

        public async Task<IActionResult> GetOne(QuestionSingleRequestModel request)
        {
            try
            {
                var entity = await Task.FromResult(_repository.QueryableDetach()
                    .Where(x => x.Id.Equals(request.Id)));

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

        public async Task<IActionResult> Post(QuestionPostRequestModel request)
        {
            try
            {
                _repository.Insert(request);
                return await Task.FromResult(new OkObjectResult(request));
            }
            catch (Exception e)
            {
                ErrorLog.Log($"Houve uma falha ao salvar {e.Message} - {e.InnerException?.Message}");
                ErrorLog.Log($"{e.StackTrace}");
                return new BadRequestObjectResult(new { message = "Houve uma falha ao salvar o registro." });
            }
        }

        public async Task<IActionResult> Put(QuestionPutRequestModel request)
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

        public async Task<IActionResult> Delete(QuestionDeleteRequestModel request)
        {
            try
            {
                var entity = _repository.GetById(request.Id);
                if (entity == null)
                    return new BadRequestObjectResult(new { message = NOT_FOUND });

                _repository.Delete(request.Id);
                _repository.Commit();

                return await Task.FromResult(new OkObjectResult(new { message = "Pergunta removida com sucesso." }));
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