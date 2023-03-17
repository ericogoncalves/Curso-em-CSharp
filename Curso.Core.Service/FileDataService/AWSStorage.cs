using Amazon.S3;
using Amazon.S3.Model;
using Curso.Core.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Curso.Core.Service.FileDataService
{
    public class AWSStorage : IStorageService
    {
        private readonly IAmazonS3 _amazonS3;
        private readonly string _bucketName;
        private readonly string _profileName;
        private readonly string _accessKey;

        public AWSStorage(IConfiguration configuration)
        {
            var _config = configuration.GetSection("Core").GetSection("FileDataSettings");
            _profileName = _config.GetValue<string>("AWSProfileName");
            _accessKey = _config.GetValue<string>("AWSAccessKey");
            _bucketName = _config.GetValue<string>("StorageBucket");

            var _secretKey = _config.GetValue<string>("AWSSecretKey");
            _amazonS3 = new AmazonS3Client(_accessKey, _secretKey);
        }

        public async Task<MemoryStream> DownloadFileAsync(string fileUrl)
        {
            try
            {
                var request = new GetObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileUrl   // + "/" + fileName
                };

                using (var getObjectResponse = await _amazonS3.GetObjectAsync(request))
                {
                    using (var responseStream = getObjectResponse.ResponseStream)
                    {
                        var stream = new MemoryStream();
                        await responseStream.CopyToAsync(stream);
                        stream.Position = 0;
                        return stream;
                    }
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

                    PutObjectRequest request = new PutObjectRequest()
                    {
                        InputStream = memoryStream,
                        BucketName = _bucketName,
                        Key = fileNameForStorage
                    };

                    PutObjectResponse response = await _amazonS3.PutObjectAsync(request);
                    if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                        return fileNameForStorage;
                    else
                        return string.Empty;
                }
            }
            catch { throw; }
        }

        public async Task DeleteFileAsync(string key)
        {
            try
            {
                DeleteObjectResponse response = await _amazonS3.DeleteObjectAsync(_bucketName, key);
                if (response.HttpStatusCode != System.Net.HttpStatusCode.NoContent)
                    throw new Exception(response.HttpStatusCode.ToString());
            }
            catch { throw; }
        }
    }
}
