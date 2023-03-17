using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Curso.Core.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace Curso.Core.Service.FileDataService
{
    public class AzureStorage : IStorageService
    {
        private readonly string _accessKey;
        private readonly string _bucketName;

        public AzureStorage(IConfiguration configuration)
        {
            var _config = configuration.GetSection("Core").GetSection("FileDataSettings");
            _accessKey = _config.GetValue<string>("AzureAccessKey");
            _bucketName = _config.GetValue<string>("StorageBucket");
        }

        public async Task<MemoryStream> DownloadFileAsync(string fileUrl)
        {
            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(_accessKey);
                BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(_bucketName);
                BlobClient blobClient = containerClient.GetBlobClient(fileUrl);
                BlobDownloadInfo download = await blobClient.DownloadAsync();

                using (var memoryStream = new MemoryStream())
                {
                    await download.Content.CopyToAsync(memoryStream);
                    return memoryStream;
                }
            }
            catch { throw; }
        }

        public async Task<string> UploadFileAsync(IFormFile formFile, string fileNameForStorage)
        {
            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(_accessKey);
                BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(_bucketName);
                BlobClient blobClient = containerClient.GetBlobClient(fileNameForStorage);

                using (var memoryStream = new MemoryStream())
                {
                    await formFile.CopyToAsync(memoryStream);
                    var result = await blobClient.UploadAsync(memoryStream, true);

                    BlobProperties properties = blobClient.GetProperties();
                    return string.Empty;
                }

            }
            catch { throw; }
        }


        public async Task DeleteFileAsync(string fileUrl)
        {
            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(_accessKey);
                BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(_bucketName);
                BlobClient blobClient = containerClient.GetBlobClient(fileUrl);
                await blobClient.DeleteAsync();
            }
            catch { throw; }
        }
    }
}
