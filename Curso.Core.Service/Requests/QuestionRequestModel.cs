using Curso.Core.Model.DataModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Curso.Core.Service.Requests
{
    public class QuestionListRequestModel : ABaseListRequestModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public List<Answer> Answers { get; set; }

    }
    public class QuestionSingleRequestModel : ABaseSingleRequestModel { }
    public class QuestionPostRequestModel : Question, IRequest<IActionResult> { }
    public class QuestionPutRequestModel : Question, IRequest<IActionResult> { }
    public class QuestionDeleteRequestModel : ABaseDeleteRequestModel { }

}