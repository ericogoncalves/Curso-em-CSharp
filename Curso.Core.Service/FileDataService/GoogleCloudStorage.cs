using Curso.Core.Service.Interfaces;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace Curso.Core.Service.FileDataService
{
    public class GoogleCloudStorage : IStorageService
    {
        private readonly GoogleCredential _googleCredential;
        private readonly StorageClient _storageClient;
        private readonly string _bucketName;

        public GoogleCloudStorage(IConfiguration configuration)
        {
            var _config = configuration.GetSection("Core").GetSection("FileDataSettings");
            _googleCredential = GoogleCredential.FromFile(_config.GetValue<string>("GoogleCredentialFile"));
            _storageClient = StorageClient.Create(_googleCredential);
            _bucketName = _config.GetValue<string>("StorageBucket");
        }

        public async Task<MemoryStream> DownloadFileAsync(string fileUrl)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await _storageClient.DownloadObjectAsync(_bucketName, fileUrl, memoryStream);
                    return memoryStream;
                }
            }
            catch { throw; }
        }

        public async Task<string> UploadFileAsync(IFormFile formFile, string fileNameForStorage)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await formFile.CopyToAsync(memoryStream);
                    var dataObject = await _storageClient.UploadObjectAsync(_bucketName, fileNameForStorage, null, memoryStream);
                    return dataObject.MediaLink;
                }
            }
            catch { throw; }
        }

        public async Task DeleteFileAsync(string fileUrl)
        {
            try
            {
                await _storageClient.DeleteObjectAsync(_bucketName, fileUrl);
            }
            catch { throw; }
        }
    }
}
