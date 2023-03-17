using DevSnap.Core.Service.Requests;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DevSnap.Core.API.Controllers
{
    public class BankingDetailsController : ABaseController<BankingDetailsController,
        BankingDetailsListRequestModel,
        BankingDetailsSingleRequestModel,
        BankingDetailsDeleteRequestModel,
        BankingDetailsPostRequestModel,
        BankingDetailsPutRequestModel>
    {
        public BankingDetailsController(ILogger<BankingDetailsController> logger, IMediator mediator) : base(logger, mediator)
        {
        }
    }
}
