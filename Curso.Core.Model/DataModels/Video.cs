using System;

namespace Curso.Core.Model.DataModels
{
    /// <summary>
    /// Video
    /// </summary>
    public class Video : BaseModel
    {
        /// <summary>
        /// Id
        /// </summary>
        //[Required(ErrorMessage = "É obrigatório informar o Id do Video")]
        public Guid Id { get; set; }

        /// <summary>
        /// Link Vimeo
        /// </summary>
        //[Required(AllowEmptyStrings =false,ErrorMessage ="É obrigatório informar o Link de Video")]
        public string Link { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        //[Required(AllowEmptyStrings = false, ErrorMessage = "É obrigatório informar o título de Video")]
        public string Title { get; set; }

        /// <summary>
        /// Subtitle
        /// </summary>
        public string Subtitle { get; set; }

        /// <summary>
        /// Short Description
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        //[Required(AllowEmptyStrings = false, ErrorMessage = "É obrigatório informar a Descrição de Video")]
        public string Description { get; set; }
        /// <summary>
        /// Order
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// FK Onboarding
        /// </summary>
        public Guid? ImageId { get; set; }
        public Guid? FileId { get; set; }
        public string ThumbId { get; set; }
        public bool IsViewed { get; set; } = false;
        public string Module { get; set; }
        public string Lenght { get; set; }
    }
}
