using Curso.Core.Service.Interfaces;
using Curso.Core.Service.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Curso.Core.Service.Handlers
{
    public class QuestionRequestHandler :
        IRequestHandler<QuestionListRequestModel, IActionResult>,
        IRequestHandler<QuestionSingleRequestModel, IActionResult>,
        IRequestHandler<QuestionDeleteRequestModel, IActionResult>,
        IRequestHandler<QuestionPostRequestModel, IActionResult>,
        IRequestHandler<QuestionPutRequestModel, IActionResult>
    {
        private readonly IQuestionService _service;

        public QuestionRequestHandler(IQuestionService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Handle(QuestionListRequestModel request, CancellationToken cancellationToken)
        {
            return await _service.GetAll(request);
        }

        public async Task<IActionResult> Handle(QuestionSingleRequestModel request, CancellationToken cancellationToken)
        {
            return await _service.GetOne(request);
        }

        public async Task<IActionResult> Handle(QuestionDeleteRequestModel request, CancellationToken cancellationToken)
        {
            return await _service.Delete(request);
        }

        public async Task<IActionResult> Handle(QuestionPostRequestModel request, CancellationToken cancellationToken)
        {
            return await _service.Post(request);
        }

        public async Task<IActionResult> Handle(QuestionPutRequestModel request, CancellationToken cancellationToken)
        {
            return await _service.Put(request);
        }

    }
}