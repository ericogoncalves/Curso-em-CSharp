using Curso.Core.Model.DataModels;
using DevSnap.CommonLibrary.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Curso.Core.Data.Mappings
{
    public class FileDataMap : EntityTypeConfiguration<FileData>
    {
        public override void Map(EntityTypeBuilder<FileData> builder)
        {
            builder.ToTable("FileData");
            builder.HasKey(x => x.Id);
        }
    }
}
