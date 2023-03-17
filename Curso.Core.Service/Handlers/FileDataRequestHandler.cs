using Curso.Core.Service.Interfaces;
using DevSnap.Core.Service.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Curso.Core.Service.Handlers
{
    public class FileDataRequestHandler :
        IRequestHandler<FileDataListRequestModel, IActionResult>,
        IRequestHandler<FileDataSingleRequestModel, IActionResult>,
        IRequestHandler<FileDataDeleteRequestModel, IActionResult>,
        IRequestHandler<FileDataPostRequestModel, IActionResult>
    {
        private readonly IService<IFormFile> _service;
        public FileDataRequestHandler(IService<IFormFile> service) => _service = service;


        public async Task<IActionResult> Handle(FileDataListRequestModel request, CancellationToken cancellationToken)
        {
            return await _service.GetAll(request);
        }

        public async Task<IActionResult> Handle(FileDataSingleRequestModel request, CancellationToken cancellationToken)
        {
            return await _service.GetOne(request);
        }

        public async Task<IActionResult> Handle(FileDataPostRequestModel request, CancellationToken cancellationToken)
        {

            return await _service.Post(request.FormFile);
        }

        public async Task<IActionResult> Handle(FileDataDeleteRequestModel request, CancellationToken cancellationToken)
        {
            return await _service.Delete(request);
        }
    }
}