using Curso.Core.Model.DataModels;

namespace Curso.Core.Data.Repositories
{
    public class QuestionRepository : ARepository<Question>
    {
        public QuestionRepository(CoreDbContext context) : base(context) { }
    }
}