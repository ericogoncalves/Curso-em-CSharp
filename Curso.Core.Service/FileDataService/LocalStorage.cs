using Curso.Core.Configuration;
using Curso.Core.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace Curso.Core.Service.FileDataService
{
    public class LocalStorage : IStorageService
    {
        private readonly string appFolder;
        private readonly string bucketName;
        public LocalStorage(IConfiguration configuration)
        {
            var _config = configuration.GetSection("FileDataSettings");
            var dataSettings = ConfigureSettings.GetFileDataSettings();
            appFolder = Directory.GetCurrentDirectory();
            bucketName = _config.GetValue<string>("StorageBucket");
            bucketName = dataSettings.StorageBucket;
        }

        public async Task<MemoryStream> DownloadFileAsync(string fileUrl)
        {
            try
            {
                byte[] fileBytes = File.ReadAllBytes(Path.Combine(appFolder, fileUrl));
                return await Task.FromResult(new MemoryStream(fileBytes));
            }
            catch { throw; }
        }

        public async Task<string> UploadFileAsync(IFormFile formFile, string fileNameForStorage)
        {
            try
            {
                if (!Directory.Exists(Path.Combine(appFolder, bucketName)))
                    Directory.CreateDirectory(Path.Combine(appFolder, bucketName));

                var fullPath = Path.Combine(appFolder, bucketName, fileNameForStorage);
                FileStream stream = new FileStream(fullPath, FileMode.Create);
                await formFile.CopyToAsync(stream);
                return Path.Combine(bucketName, fileNameForStorage);
            }
            catch { throw; }
        }
        public async Task DeleteFileAsync(string fileUrl)
        {
            try
            {
                await Task.Run(() => File.Delete(Path.Combine(appFolder, fileUrl)));
            }
            catch { throw; }
        }
    }
}