using Curso.Core.Api.Controllers;
using Curso.Core.Service.Requests;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Curso.Core.API.Controllers
{
    public class AnswerController : ABaseController<AnswerController,
        AnswerListRequestModel,
        AnswerSingleRequestModel,
        AnswerDeleteRequestModel,
        AnswerPostViewModel,
        AnswerPutRequestModel>
    {
        public AnswerController(ILogger<AnswerController> logger, IMediator mediator) : base(logger, mediator)
        {
        }
    }
}