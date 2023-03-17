using Curso.Core.Model.DataModels;
using DevSnap.CommonLibrary.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Curso.Core.Data.Mappings
{
    public class AnswerMap : EntityTypeConfiguration<Answer>
    {
        public override void Map(EntityTypeBuilder<Answer> builder)
        {
            builder.ToTable("Answers");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.PermitionAccessId);

            //builder.Property(x => x.QuestionId);

            //builder.HasOne(x => x.Question)
            //       .WithMany(x => x.Answers)
            //       .HasForeignKey(x => x.QuestionId)
            //       .OnDelete(DeleteBehavior.Cascade);

        }
    }
}