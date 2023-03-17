using Curso.Core.Data.Interfaces;
using Curso.Core.Model.DataModels;
using Curso.Core.Service.Interfaces;
using Curso.Core.Service.Requests;
using DevSnap.Core.Service.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;


namespace Curso.Core.Service.Services
{
    public class FileDataService : IService<IFormFile>
    {
        private readonly IARepository<FileData> _repository;
        private readonly IStorageService _storage;
        public FileDataService(IARepository<FileData> repository, IStorageService storage)
        {
            _repository = repository;
            _storage = storage;
        }

        public async Task<IActionResult> GetOne(ABaseSingleRequestModel request)
        {
            var entity = await Task.FromResult(_repository.GetById(request.Id));
            if (entity == null)
                return new NotFoundObjectResult("File not found.");

            var content = await _storage.DownloadFileAsync(entity.FileUrl);
            return new OkObjectResult(new DownloadFileResponse()
            {
                FileName = entity.FileName,
                Content = content
            });
        }

        public async Task<IActionResult> Post(IFormFile request)
        {
            try
            {
                Guid guid = Guid.NewGuid();
                string fileName = guid.ToString() + "_" + request.FileName;

                var fileUrl = await _storage.UploadFileAsync(request, fileName);
                var entity = new FileData()
                {
                    Id = guid,
                    FileName = request.FileName,
                    FileUrl = fileUrl,
                    DateCreated = DateTime.UtcNow,
                    StorageType = _storage.GetType().Name
                };

                _repository.Insert(entity);
                return new OkObjectResult(entity);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        public async Task<IActionResult> Delete(ABaseDeleteRequestModel request)
        {
            try
            {
                var entity = _repository.GetById(request.Id);
                if (entity == null)
                    return new NotFoundObjectResult("File not found.");

                await _storage.DeleteFileAsync(entity.FileUrl);
                _repository.Delete(request.Id);
                _repository.Commit();

                return new NoContentResult();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        #region Not Implemented..
        public Task<IActionResult> GetAll(ABaseListRequestModel request)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Put(IFormFile request)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
