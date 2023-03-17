using Curso.Core.Service.Interfaces;
using Curso.Core.Service.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Curso.Core.Service.Handlers
{
    public class PermitionAccessRequestHandler :
        IRequestHandler<PermitionAccessGetAllRequestModel, IActionResult>,
        IRequestHandler<PermitionAccessGetOneRequestModel, IActionResult>,
        IRequestHandler<PermitionAccessDeleteRequestModel, IActionResult>,
        IRequestHandler<PermitionAccessPostRequestModel, IActionResult>,
        IRequestHandler<PermitionAccessPutRequestModel, IActionResult>
    {
        private readonly IPermitionAccessService _service;

        public PermitionAccessRequestHandler(IPermitionAccessService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Handle(PermitionAccessGetAllRequestModel request, CancellationToken cancellationToken)
        {
            return await _service.GetAll(request);
        }

        public async Task<IActionResult> Handle(PermitionAccessGetOneRequestModel request, CancellationToken cancellationToken)
        {
            return await _service.GetOne(request);
        }

        public async Task<IActionResult> Handle(PermitionAccessDeleteRequestModel request, CancellationToken cancellationToken)
        {
            return await _service.Delete(request);
        }

        public async Task<IActionResult> Handle(PermitionAccessPostRequestModel request, CancellationToken cancellationToken)
        {
            return await _service.Post(request);
        }

        public async Task<IActionResult> Handle(PermitionAccessPutRequestModel request, CancellationToken cancellationToken)
        {
            return await _service.Put(request);
        }
        
        

    }
}