using Curso.Core.Api.Controllers;
using Curso.Core.Service.Requests;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Curso.Core.API.Controllers
{
    public class QuestionController : ABaseController<QuestionController,
        QuestionListRequestModel,
        QuestionSingleRequestModel,
        QuestionDeleteRequestModel,
        QuestionPostRequestModel,
        QuestionPutRequestModel>
    {
        public QuestionController(ILogger<QuestionController> logger, IMediator mediator) : base(logger, mediator)
        {
        }
    }
}