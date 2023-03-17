using Curso.Core.Model.DataModels;

namespace Curso.Core.Data.Repositories
{
    public class VideoRepository : ARepository<Video>
    {
        public VideoRepository(CoreDbContext context) : base(context) { }
    }
}
