using DevSnap.Core.Service.Requests;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DevSnap.Core.API.Controllers
{
    public class AddressController : ABaseController<AddressController,
        AddressListRequestModel,
        AddressSingleRequestModel,
        AddressDeleteRequestModel,
        AddressPostRequestModel,
        AddressPutRequestModel>
    {
        public AddressController(ILogger<AddressController> logger, IMediator mediator) : base(logger, mediator)
        {
        }
    }
}
