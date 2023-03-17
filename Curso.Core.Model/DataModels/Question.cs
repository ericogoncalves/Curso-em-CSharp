using System;
using System.Collections.Generic;

namespace Curso.Core.Model.DataModels
{
    public class Question : BaseModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public Guid PermitionAccessId { get; set; }
        public List<Answer> Answers { get; set; }
    }
}