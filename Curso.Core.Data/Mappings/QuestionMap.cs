using Curso.Core.Model.DataModels;
using DevSnap.CommonLibrary.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Curso.Core.Data.Mappings
{
    public class QuestionMap : EntityTypeConfiguration<Question>
    {
        public override void Map(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Querys");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Description)
                 .HasMaxLength(250)
                 .IsRequired();
        }
    }
}