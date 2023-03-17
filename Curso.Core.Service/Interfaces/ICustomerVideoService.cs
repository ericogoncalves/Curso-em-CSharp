using Curso.Core.Model.DataModels;
using DevSnap.Core.Service.Requests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevSnap.Core.Service.Interfaces
{
    public interface ICustomerVideoService
    {
        Task<IActionResult> GetAll(CustomerVideoListRequestModel request);
        Task<IActionResult> GetOne(CustomerVideoSingleRequestModel request);
        Task<IActionResult> Post(CustomerVideoPostRequestModel request);
        Task<IActionResult> Put(CustomerVideoPutRequestModel request);
        Task<IActionResult> Delete(CustomerVideoDeleteRequestModel request);
        List<CustomerVideo> GetCustomerVideos(Guid PermitionAccessId, Guid? videoId = null);
    }
}