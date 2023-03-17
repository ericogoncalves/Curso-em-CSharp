using System;

namespace Curso.Core.Model.DataModels
{
    public class FileData : BaseModel
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public DateTime DateCreated { get; set; }
        public string StorageType { get; set; }
    }
}
