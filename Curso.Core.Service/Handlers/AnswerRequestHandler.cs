using Curso.Core.Service.Interfaces;
using Curso.Core.Service.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Curso.Core.Service.Handlers
{
    public class AnswerRequestHandler :
        IRequestHandler<AnswerListRequestModel, IActionResult>,
        IRequestHandler<AnswerSingleRequestModel, IActionResult>,
        IRequestHandler<AnswerDeleteRequestModel, IActionResult>,
        IRequestHandler<AnswerPostViewModel, IActionResult>,
        IRequestHandler<AnswerPutRequestModel, IActionResult>
    {
        private readonly IAnswerService _service;

        public AnswerRequestHandler(IAnswerService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Handle(AnswerListRequestModel request, CancellationToken cancellationToken)
        {
            return await _service.GetAll(request);
        }

        public async Task<IActionResult> Handle(AnswerSingleRequestModel request, CancellationToken cancellationToken)
        {
            return await _service.GetOne(request);
        }

        public async Task<IActionResult> Handle(AnswerDeleteRequestModel request, CancellationToken cancellationToken)
        {
            return await _service.Delete(request);
        }

        public async Task<IActionResult> Handle(AnswerPostViewModel request, CancellationToken cancellationToken)
        {
            return await _service.Post(request);
        }

        public async Task<IActionResult> Handle(AnswerPutRequestModel request, CancellationToken cancellationToken)
        {
            return await _service.Put(request);
        }

    }
}