using AutoMapper;
using Curso.Core.Data;
using Curso.Core.Data.Interfaces;
using Curso.Core.Model.DataModels;
using Curso.Core.Service.Interfaces;
using Curso.Core.Service.Requests;
using DevSnap.CommonLibrary.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SendMail;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Core.Service.Services
{
    public class PermitionAccessService : IPermitionAccessService
    {
        private readonly IARepository<PermitionAccess> _repository;
        private readonly CoreDbContext _context;
        private readonly IMapper _mapper;
        private const string NOT_FOUND = "Nenhum dado encontrado.";

        public PermitionAccessService(IARepository<PermitionAccess> repository,
            CoreDbContext context,
            IMapper mapper)
        {
            _repository = repository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> GetAll(PermitionAccessGetAllRequestModel request)
        {
            try
            {
                var items = await Task.FromResult(_repository
                    .QueryableDetach()
                );

                if (!string.IsNullOrEmpty(request.Name))
                    items = items.Where(x => x.Name.ToLower().Contains(request.Name.ToLower()));

                if (!string.IsNullOrEmpty(request.Email))
                    items = items.Where(x => x.Email.ToLower().Contains(request.Email.ToLower()));

                if (!string.IsNullOrEmpty(request.Search))
                    items = items.Where(x => x.Name.ToLower().Contains(request.Search.ToLower()) || x.Email.ToLower().Contains(request.Search.ToLower()));

                items = items.OrderByDescending(x => x.Id);

                return request.PageSize != 0
                    ? new OkObjectResult(Pagination<PermitionAccess>.Create(items, request.CurrentPage, request.PageSize))
                    : new OkObjectResult(items.ToList());

            }
            catch (Exception e)
            {
                ErrorLog.Log($"Houve uma falha ao buscar informações {e.Message} - {e.InnerException?.Message}");
                ErrorLog.Log($"{e.StackTrace}");
                return new BadRequestObjectResult(new { message = "Houve uma falha ao buscar as informações." });
            }
        }

        public async Task<IActionResult> GetOne(PermitionAccessGetOneRequestModel request)
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

        public async Task<IActionResult> Post(PermitionAccessPostRequestModel request)
        {
            try
            {
                var obj = new PermitionAccess
                {
                    Id = (request.Id == Guid.Empty) ? Guid.NewGuid() : request.Id,
                    Name = request.Name,
                    Email = request.Email,
                    Phone = request.Phone,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                request.Password = CreatePassword(6);

                obj.Password = request.Password;

                _context.Entry(obj).State = EntityState.Added;
                _context.SaveChanges();

                new SendGridService().SendMailCourse(obj.Name, obj.Email, obj.Password);

                return await Task.FromResult(new OkObjectResult(new { message = "Usuário cadastrado com sucesso." }));
            }
            catch (Exception ex)
            {
                ErrorLog.Log($"Erro ao realizar a ação {ex.Message} - {ex.InnerException?.Message}");
                ErrorLog.Log(ex.StackTrace);
                return new BadRequestObjectResult(new { message = "Usuário não foi cadastrado" });
            }
        }
        private string CreatePassword(int length)
        {
            const string valid = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }


        public async Task<IActionResult> Put(PermitionAccessPutRequestModel request)
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

        public async Task<IActionResult> Delete(PermitionAccessDeleteRequestModel request)
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

        public async Task<IActionResult> GeneratePasswordAccess(GeneratePasswordAccessRequestModel request)
        {
            try
            {
                var entity = await Task.FromResult(_repository.QueryableDetach()
                    .Where(x => x.Email.ToLower() == request.Email.ToLower())
                    .FirstOrDefault());

                if (entity == null)
                    return new BadRequestObjectResult(new { message = NOT_FOUND });

                entity.Password = CreatePassword(6);

                _repository.Update(entity);
                _repository.Commit();

                new SendGridService().SendMailCourse(entity.Name, entity.Email, entity.Password);


                return await Task.FromResult(new OkObjectResult(request));
            }
            catch (Exception e)
            {
                ErrorLog.Log($"Houve uma falha ao atualizar {e.Message} - {e.InnerException?.Message}");
                ErrorLog.Log($"{e.StackTrace}");
                return new BadRequestObjectResult(new { message = "Houve uma falha ao atualizar o registro." });
            }
        }

        public async Task<IActionResult> ValidationPassword(ValidationPasswordRequestModel request)
        {
            try
            {
                var entity = await Task.FromResult(_repository.QueryableDetach()
                    .Where(x => x.Password.ToLower() == request.Password.ToLower() && x.Email.ToLower() == request.Email.ToLower())
                    .FirstOrDefault());

                if (entity == null)
                {
                    return new BadRequestObjectResult(new { message = "Email ou Código inválidos." });
                }

                return await Task.FromResult(new OkObjectResult(new { entity.Id }));
            }
            catch (Exception e)
            {
                ErrorLog.Log($"Houve uma falha ao buscar informações {e.Message} - {e.InnerException?.Message}");
                ErrorLog.Log($"{e.StackTrace}");
                return new BadRequestObjectResult(new { message = "Houve uma falha ao buscar as informações." });
            }
        }
    }
}