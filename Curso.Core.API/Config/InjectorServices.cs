using Curso.Core.Data.Interfaces;
using Curso.Core.Data.Repositories;
using Curso.Core.Model.DataModels;
using Curso.Core.Service.Interfaces;
using Curso.Core.Service.Services;
using DevSnap.Core.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Curso.Core.Api
{
    public static class InjectorServices
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            #region "Repository"
            //services.AddScoped<IARepository<CustomerObjective>, CustomerObjectiveRepository>();
            services.AddScoped<IARepository<Question>, QuestionRepository>();
            services.AddScoped<IARepository<Answer>, AnswerRepository>();
            services.AddScoped<IARepository<PermitionAccess>, PermitionAccessRepository>();
            services.AddScoped<IARepository<Video>, VideoRepository>();
            services.AddScoped<IARepository<CustomerVideo>, CustomerVideoRepository>();
            services.AddScoped<IARepository<FileData>, FileDataRepository>();
            #endregion

            #region "Service"
            //services.AddScoped<ICustomerObjectiveService, CustomerObjectiveService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IAnswerService, AnswerService>();
            services.AddScoped<IPermitionAccessService, PermitionAccessService>();
            services.AddScoped<IVideoService, VideoService>();
            services.AddScoped<ICustomerVideoService, CustomerVideoService>();
            services.AddScoped<IService<IFormFile>, FileDataService>();
            #endregion
        }
    }
}
