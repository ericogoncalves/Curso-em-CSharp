using Curso.Core.Service.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Curso.Core.Service.Interfaces
{
    public interface IPermitionAccessService
    {
        Task<IActionResult> GetAll(PermitionAccessGetAllRequestModel request);
        Task<IActionResult> GetOne(PermitionAccessGetOneRequestModel request);
        Task<IActionResult> Post(PermitionAccessPostRequestModel request);
        Task<IActionResult> Put(PermitionAccessPutRequestModel request);
        Task<IActionResult> Delete(PermitionAccessDeleteRequestModel request);
     
    }
}