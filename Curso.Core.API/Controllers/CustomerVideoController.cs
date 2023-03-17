using Curso.Core.Api.Controllers;
using DevSnap.Core.Service.Requests;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Curso.Core.API.Controllers
{
    // [Authorize]
    public class CustomerVideoController : ABaseController<CustomerVideoController,
        CustomerVideoListRequestModel,
        CustomerVideoSingleRequestModel,
        CustomerVideoDeleteRequestModel,
        CustomerVideoPostRequestModel,
        CustomerVideoPutRequestModel>
    {
        public CustomerVideoController(ILogger<CustomerVideoController> logger, IMediator mediator) : base(logger, mediator)
        {
        }
    }
}