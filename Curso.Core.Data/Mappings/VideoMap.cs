using Curso.Core.Model.DataModels;
using DevSnap.CommonLibrary.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Curso.Core.Data.Mappings
{
    public class VideoMap : EntityTypeConfiguration<Video>
    {
        public override void Map(EntityTypeBuilder<Video> builder)
        {
            builder.ToTable("Videos");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

        }
    }
}
