using Curso.Core.Model.DataModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Curso.Core.Service.Requests
{
    public class AnswerListRequestModel : ABaseListRequestModel
    {
        public Guid? PermitionAccessId { get; set; }
    }
    public class AnswerSingleRequestModel : ABaseSingleRequestModel { }
    public class AnswerPostRequestModel : Answer, IRequest<IActionResult> { }
    public class AnswerPutRequestModel : Answer, IRequest<IActionResult> { }
    public class AnswerDeleteRequestModel : ABaseDeleteRequestModel { }
    public class AnswerPostViewModel : IRequest<IActionResult>
    {
        public List<AnswerViewModel> Answers { get; set; }
        public Guid PermitionAccessId { get; set; }

    }
    public class AnswerViewModel : IRequest<IActionResult>
    {
        public string Question { get; set; }
        public string Response { get; set; }
        public Guid PermitionAccessId { get; set; }
    }

}