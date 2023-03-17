using Curso.Core.Service.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace DevSnap.Core.Service.Requests
{
    public class FileDataListRequestModel : ABaseListRequestModel { }
    public class FileDataSingleRequestModel : ABaseSingleRequestModel { }
    public class FileDataDeleteRequestModel : ABaseDeleteRequestModel { }
    public class FileDataPostRequestModel : IRequest<IActionResult>
    {
        public IFormFile FormFile { get; set; }
    }
    public class DownloadFileResponse
    {
        public string FileName { get; set; }
        public MemoryStream Content { get; set; }
    }
}
