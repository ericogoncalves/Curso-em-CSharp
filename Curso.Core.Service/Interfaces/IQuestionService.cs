using Curso.Core.Service.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Curso.Core.Service.Interfaces
{
    public interface IQuestionService
    {
        Task<IActionResult> GetAll(QuestionListRequestModel request);
        Task<IActionResult> GetOne(QuestionSingleRequestModel request);
        Task<IActionResult> Post(QuestionPostRequestModel request);
        Task<IActionResult> Put(QuestionPutRequestModel request);
        Task<IActionResult> Delete(QuestionDeleteRequestModel request);
    }
}