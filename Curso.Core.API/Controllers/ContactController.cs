using DevSnap.Core.Service.Requests;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DevSnap.Core.API.Controllers
{
    public class ContactController : ABaseController<ContactController,
        ContactListRequestModel,
        ContactSingleRequestModel,
        ContactDeleteRequestModel,
        ContactPostRequestModel,
        ContactPutRequestModel>
    {
        public ContactController(ILogger<ContactController> logger, IMediator mediator) : base(logger, mediator)
        {
        }
    }
}
