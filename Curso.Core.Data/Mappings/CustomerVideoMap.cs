using Curso.Core.Model.DataModels;
using DevSnap.CommonLibrary.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Curso.Core.Data.Mappings
{
    public class CustomerVideoMap : EntityTypeConfiguration<CustomerVideo>
    {
        public override void Map(EntityTypeBuilder<CustomerVideo> builder)
        {
            builder.ToTable("CustomerVideos");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(x => x.Video)
                .WithMany()
                .HasForeignKey(x => x.VideoId);
        }
    }
}