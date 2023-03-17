using Curso.Core.Model.DataModels;

namespace Curso.Core.Data.Repositories
{
    public class FileDataRepository : ARepository<FileData>
    {
        public FileDataRepository(CoreDbContext context) : base(context) { }
    }
}
