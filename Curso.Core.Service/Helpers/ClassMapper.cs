using AutoMapper;
using Curso.Core.Model.DataModels;
using Curso.Core.Service.Requests;
using DevSnap.Core.Service.Requests;
using System.Collections.Generic;

namespace Curso.Core.Service
{
    public class ClassMapper : Profile
    {
        public ClassMapper()
        {
            #region "Question"
            CreateMap<Question, QuestionListRequestModel>().ReverseMap();
            CreateMap<Question, QuestionSingleRequestModel>().ReverseMap();
            CreateMap<Question, QuestionPostRequestModel>().ReverseMap();
            CreateMap<Question, QuestionPutRequestModel>().ReverseMap();
            CreateMap<Question, QuestionDeleteRequestModel>().ReverseMap();
            #endregion

            #region "Answer"
            CreateMap<Answer, AnswerListRequestModel>().ReverseMap();
            CreateMap<Answer, AnswerSingleRequestModel>().ReverseMap();
            CreateMap<Answer, AnswerPostRequestModel>().ReverseMap();
            CreateMap<Answer, AnswerPutRequestModel>().ReverseMap();
            CreateMap<Answer, AnswerDeleteRequestModel>().ReverseMap();
            #endregion

            #region "PermitionAccess"
            CreateMap<PermitionAccess, PermitionAccessGetAllRequestModel>().ReverseMap();
            CreateMap<PermitionAccess, PermitionAccessGetOneRequestModel>().ReverseMap();
            CreateMap<PermitionAccess, PermitionAccessPostRequestModel>().ReverseMap();
            CreateMap<PermitionAccess, PermitionAccessPutRequestModel>().ReverseMap();
            CreateMap<PermitionAccess, PermitionAccessDeleteRequestModel>().ReverseMap();
            #endregion

            #region "Video"
            CreateMap<Video, VideoSingleRequestModel>().ReverseMap();
            CreateMap<Video, VideoPostRequestModel>().ReverseMap();
            CreateMap<Video, VideoListRequestModel>().ReverseMap();
            CreateMap<Video, VideoPutRequestModel>().ReverseMap();
            CreateMap<Video, VideoDeleteRequestModel>().ReverseMap();
            CreateMap<Video, VideoResponseModel>().ReverseMap();
            CreateMap<List<Video>, List<VideoListRequestModel>>().ReverseMap();
            #endregion

            #region "CustomerVideo"
            CreateMap<CustomerVideo, CustomerVideoSingleRequestModel>().ReverseMap();
            CreateMap<CustomerVideo, CustomerVideoPostRequestModel>().ReverseMap();
            CreateMap<CustomerVideo, CustomerVideoListRequestModel>().ReverseMap();
            CreateMap<CustomerVideo, CustomerVideoPutRequestModel>().ReverseMap();
            CreateMap<CustomerVideo, CustomerVideoDeleteRequestModel>().ReverseMap();
            CreateMap<List<CustomerVideo>, List<VideoListRequestModel>>().ReverseMap();
            #endregion

        }
    }
}
