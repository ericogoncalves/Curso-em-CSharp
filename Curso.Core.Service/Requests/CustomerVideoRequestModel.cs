using Curso.Core.Model.DataModels;
using Curso.Core.Service.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DevSnap.Core.Service.Requests
{
    public class CustomerVideoListRequestModel : ABaseListRequestModel
    {
        public Guid PermitionAccessId { get; set; }
    }
    public class CustomerVideoSingleRequestModel : ABaseSingleRequestModel { }
    public class CustomerVideoPostRequestModel : CustomerVideo, IRequest<IActionResult> { }
    public class CustomerVideoPutRequestModel : CustomerVideo, IRequest<IActionResult> { }
    public class CustomerVideoDeleteRequestModel : ABaseDeleteRequestModel { }
}