
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration;

public class TypeFilesConfiguration: IEntityTypeConfiguration<TypeFile>
{
    public void Configure(EntityTypeBuilder<TypeFile> builder)
    {
        builder.ToTable("typeFile");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Description)
        .HasColumnName("description")
        .HasColumnType("varchar")
        .IsRequired()
        .HasMaxLength(250);

    }
}
