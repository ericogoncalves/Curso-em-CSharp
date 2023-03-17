using Curso.Core.Model.DataModels;

namespace Curso.Core.Data.Repositories
{
    public class PermitionAccessRepository : ARepository<PermitionAccess>
    {
        public PermitionAccessRepository(CoreDbContext context) : base(context) { }
    }
}