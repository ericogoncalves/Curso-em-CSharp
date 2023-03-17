using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Curso.Core.Service.Interfaces
{
    public interface IStorageService
    {
        Task<MemoryStream> DownloadFileAsync(string fileUrl);
        Task<string> UploadFileAsync(IFormFile formFile, string fileNameForStorage);
        Task DeleteFileAsync(string fileUrl);
    }
}
