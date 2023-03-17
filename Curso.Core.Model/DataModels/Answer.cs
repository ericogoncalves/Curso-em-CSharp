
using System;

namespace Curso.Core.Model.DataModels
{
    public class Answer : BaseModel
    {
        public int Id { get; set; }
        //public string QuestionId { get; set; }
        //public Question Question { get; set; }
        public string Question { get; set; }
        public string Response { get; set; }
        public Guid PermitionAccessId { get; set; }

    }
}