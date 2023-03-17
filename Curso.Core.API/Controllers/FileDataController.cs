using Curso.Core.Api;
using DevSnap.Core.Service.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Curso.Core.API.Controllers
{
    //[Authorize]
    [Microsoft.AspNetCore.Components.Route("api/[controller]")]
    [ApiController]
    public class FileDataController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly CoreSettings _config;

        public FileDataController(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _config = configuration.GetSection("Core").Get<CoreSettings>();
        }


        [HttpGet("Download/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var request = new FileDataSingleRequestModel { Id = id };
                var response = await _mediator.Send(request);
                DownloadFileResponse file = (DownloadFileResponse)((ObjectResult)response).Value;

                var contentType = "application/octet-stream";
                return File(file.Content, contentType, file.FileName);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { message = ex.Message });
            }
        }

        // POST: api/FileData
        [HttpPost("Upload")]
        public async Task<IActionResult> Post(IFormFile file)
        {
            try
            {
                return await _mediator.Send(new FileDataPostRequestModel { FormFile = file });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var request = new FileDataDeleteRequestModel { Id = id };
                return await _mediator.Send(request);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
