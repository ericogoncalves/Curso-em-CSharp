using System;
using System.Collections.Generic;

namespace Curso.Core.Model.DataModels
{
    public class PermitionAccess : BaseModel
    {
        public Guid Id { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<CustomerVideo> CustomerVideos { get; set; }
        public List<Question> Questions { get; set; }
        public List<Answer> Answers { get; set; }

    }
}