using System;

namespace Curso.Core.Service.Responses
{
    public class VideoResponse
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

    }
}
