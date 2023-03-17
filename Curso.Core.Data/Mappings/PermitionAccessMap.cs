using Curso.Core.Model.DataModels;
using DevSnap.CommonLibrary.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Curso.Core.Data.Mappings
{
    public class PermitionAccessMap : EntityTypeConfiguration<PermitionAccess>
    {
        public override void Map(EntityTypeBuilder<PermitionAccess> builder)
        {
            builder.ToTable("PermitionAccesses");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Name)
                 .HasMaxLength(250)
                 .IsRequired();
            builder.Property(x => x.Email)
                .HasMaxLength(250)
                .IsRequired();
            builder.Property(x => x.Phone)
                .HasMaxLength(25)
                .IsRequired();
            builder.Property(x => x.Password)
               .HasMaxLength(25)
               .IsRequired();




        }
    }
}