using Curso.Core.Model.DataModels;

namespace Curso.Core.Data.Repositories
{
    public class CustomerVideoRepository : ARepository<CustomerVideo>
    {
        public CustomerVideoRepository(CoreDbContext context) : base(context) { }
    }
}
