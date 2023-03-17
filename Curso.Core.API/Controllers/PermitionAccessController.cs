using Curso.Core.Api.Controllers;
using Curso.Core.Service.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Curso.Core.API.Controllers
{
    public class PermitionAccessController : ABaseController<PermitionAccessController,
        PermitionAccessGetAllRequestModel,
        PermitionAccessGetOneRequestModel,
        PermitionAccessDeleteRequestModel,
        PermitionAccessPostRequestModel,
        PermitionAccessPutRequestModel>
    {
        public PermitionAccessController(ILogger<PermitionAccessController> logger, IMediator mediator) : base(logger, mediator)
        {
        }

        [HttpGet("ValidationCode")]
        public virtual async Task<IActionResult> ValidationCode([FromQuery] ValidationCodeRequestModel request)
        {
            return await _mediator.Send(request);
        }

        [HttpPost("GenerateCodeAccess")]
        public virtual async Task<IActionResult> GenerateCodeAccess([FromQuery] GenerateCodeAccessRequestModel request)
        {
            return await _mediator.Send(request);
        }
    }
}