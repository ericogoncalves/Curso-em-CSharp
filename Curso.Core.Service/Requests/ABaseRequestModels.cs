using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Curso.Core.Service.Requests
{
    public abstract class ABaseListRequestModel : IRequest<IActionResult>
    {
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = int.MaxValue;
        public string Search { get; set; } = string.Empty;
    }

    public abstract class ABaseSingleRequestModel : IRequest<IActionResult>
    {
        public object Id { get; set; }
    }

    public abstract class ABaseDeleteRequestModel : IRequest<IActionResult>
    {
        public object Id { get; set; }
    }

    public abstract class ABasePostRequestModel<T> : IRequest<IActionResult>
    {
        public T Data { get; set; }
    }

    public abstract class ABasePutRequestModel<T> : IRequest<IActionResult>
    {
        public T Data { get; set; }
    }
}
