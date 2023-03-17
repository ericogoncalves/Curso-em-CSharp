namespace Curso.Core.Api
{
    public class FileDataSettings
    {
        public EFileDataStorage FileDataStorage { get; set; }
        public string StorageBucket { get; set; }

        public string AWSProfileName { get; set; }
        public string AWSAccessKey { get; set; }
        public string AWSSecretKey { get; set; }
        public string AzureAccessKey { get; set; }
        public string GoogleCredentialFile { get; set; }
    }

    public enum EFileDataStorage : byte
    {
        Local = 0,
        AWS = 1,
        Azure = 2,
        CGP = 3
    }
}
