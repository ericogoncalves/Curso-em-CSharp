using Curso.Core.Model.DataModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Curso.Core.Service.Requests
{
    public class PermitionAccessGetAllRequestModel : ABaseListRequestModel
    {
        public Guid Id { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class PermitionAccessGetOneRequestModel : ABaseSingleRequestModel { }
    public class PermitionAccessPostRequestModel : PermitionAccess, IRequest<IActionResult> { }
    public class PermitionAccessPutRequestModel : PermitionAccess, IRequest<IActionResult> { }
    public class PermitionAccessDeleteRequestModel : ABaseDeleteRequestModel { }
    public class PermitionAccessPostViewModel : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
    public class GeneratePasswordAccessRequestModel : IRequest<IActionResult>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class ValidationPasswordRequestModel : IRequest<IActionResult>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}