using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Curso.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public abstract class ABaseController<T, TA, TO, TD, TP, TU> : ControllerBase
            where T : ControllerBase
            where TA : IRequest<IActionResult>
            where TO : IRequest<IActionResult>
            where TD : IRequest<IActionResult>
            where TP : IRequest<IActionResult>
            where TU : IRequest<IActionResult>
    {
        private readonly ILogger<T> _logger;
        protected IMediator _mediator;

        protected ABaseController(ILogger<T> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetAll([FromQuery] TA request)
        {
            return await _mediator.Send(request);
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetOne([FromRoute] string id)
        {
            TO request = (TO)Activator.CreateInstance(typeof(TO));

            if (int.TryParse(id, out int intId))
                request.GetType().GetProperty("Id").SetValue(request, intId);
            else if (Guid.TryParse(id, out Guid guidId))
                request.GetType().GetProperty("Id").SetValue(request, guidId);
            else
                request.GetType().GetProperty("Id").SetValue(request, id);

            return await _mediator.Send(request);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] TP request)
        {
            return await _mediator.Send(request);
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Put([FromRoute] string id, [FromBody] TU request)
        {
            var oid = request?.GetType().GetProperty("Id")?.GetValue(request)?.ToString();
            if (id.Equals(oid))
                return await _mediator.Send(request);

            return BadRequest(new { Message = "Dados inconsistentes" });
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete([FromRoute] string id)
        {
            TD request = (TD)Activator.CreateInstance(typeof(TD));

            if (int.TryParse(id, out int intId))
                request.GetType().GetProperty("Id").SetValue(request, intId);
            else if (Guid.TryParse(id, out Guid guidId))
                request.GetType().GetProperty("Id").SetValue(request, guidId);
            else
                request.GetType().GetProperty("Id").SetValue(request, id);

            return await _mediator.Send(request);
        }

        protected string GetUserIdAuthenticated()
        {
            try
            {
                return User.Claims.First(i => i.Type == "jti").Value;
            }
            catch
            {
                ErrorLog.Log($"Não foi possível obter o id do usuário ao realizar essa operação");
            }
            return string.Empty;
        }

        protected void GerarLogEvidencia(string mensagem)
        {
            string userId = GetUserIdAuthenticated();
            ErrorLog.Log(mensagem + " " + "o usuário que realizou essa ação foi " + userId);
        }
    }
}