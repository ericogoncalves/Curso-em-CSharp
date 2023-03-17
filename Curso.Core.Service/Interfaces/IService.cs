using Curso.Core.Service.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Curso.Core.Service.Interfaces
{
    public interface IService<T> where T : class
    {
        Task<IActionResult> GetAll(ABaseListRequestModel request);
        Task<IActionResult> GetOne(ABaseSingleRequestModel request);
        Task<IActionResult> Post(T request);
        Task<IActionResult> Put(T request);
        Task<IActionResult> Delete(ABaseDeleteRequestModel request);
    }
}
