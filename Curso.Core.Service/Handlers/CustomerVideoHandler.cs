using DevSnap.Core.Service.Interfaces;
using DevSnap.Core.Service.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Curso.Core.Service.Handlers
{
    public class CustomerVideoRequestHandler :
        IRequestHandler<CustomerVideoListRequestModel, IActionResult>,
        IRequestHandler<CustomerVideoSingleRequestModel, IActionResult>,
        IRequestHandler<CustomerVideoDeleteRequestModel, IActionResult>,
        IRequestHandler<CustomerVideoPostRequestModel, IActionResult>,
        IRequestHandler<CustomerVideoPutRequestModel, IActionResult>
    {
        private readonly ICustomerVideoService _service;

        public CustomerVideoRequestHandler(ICustomerVideoService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Handle(CustomerVideoListRequestModel request, CancellationToken cancellationToken)
        {
            return await _service.GetAll(request);
        }

        public async Task<IActionResult> Handle(CustomerVideoSingleRequestModel request, CancellationToken cancellationToken)
        {
            return await _service.GetOne(request);
        }

        public async Task<IActionResult> Handle(CustomerVideoDeleteRequestModel request, CancellationToken cancellationToken)
        {
            return await _service.Delete(request);
        }

        public async Task<IActionResult> Handle(CustomerVideoPostRequestModel request, CancellationToken cancellationToken)
        {
            return await _service.Post(request);
        }

        public async Task<IActionResult> Handle(CustomerVideoPutRequestModel request, CancellationToken cancellationToken)
        {
            return await _service.Put(request);
        }
    }
}