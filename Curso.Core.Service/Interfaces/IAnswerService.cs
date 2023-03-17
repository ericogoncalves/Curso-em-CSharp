using Curso.Core.Service.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Curso.Core.Service.Interfaces
{
    public interface IAnswerService
    {
        Task<IActionResult> GetAll(AnswerListRequestModel request);
        Task<IActionResult> GetOne(AnswerSingleRequestModel request);
        Task<IActionResult> Post(AnswerPostViewModel request);
        Task<IActionResult> Put(AnswerPutRequestModel request);
        Task<IActionResult> Delete(AnswerDeleteRequestModel request);
    }
}