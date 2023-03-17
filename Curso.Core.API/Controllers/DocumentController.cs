using DevSnap.Core.Service.Requests;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DevSnap.Core.API.Controllers
{
    public class DocumentController : ABaseController<DocumentController,
        DocumentListRequestModel,
        DocumentSingleRequestModel,
        DocumentDeleteRequestModel,
        DocumentPostRequestModel,
        DocumentPutRequestModel>
    {
        public DocumentController(ILogger<DocumentController> logger, IMediator mediator) : base(logger, mediator)
        {
        }
    }
}
