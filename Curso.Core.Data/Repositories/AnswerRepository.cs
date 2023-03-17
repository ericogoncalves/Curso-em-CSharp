using Curso.Core.Model.DataModels;

namespace Curso.Core.Data.Repositories
{
    public class AnswerRepository : ARepository<Answer>
    {
        public AnswerRepository(CoreDbContext context) : base(context) { }
    }
}