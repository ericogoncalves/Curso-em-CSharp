using System;

namespace Curso.Core.Model.DataModels
{
    public class CustomerVideo : BaseModel
    {
        public Guid Id { get; set; }
        public Guid PermitionAccessId { get; set; }
        public Video Video { get; set; }
        public Guid VideoId { get; set; }
        public bool IsViewed { get; set; } = true;
    }
}