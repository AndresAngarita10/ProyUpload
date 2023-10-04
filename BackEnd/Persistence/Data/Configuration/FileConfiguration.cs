
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Persistence.Data.Configuration;

public class FileUploadConfiguration : IEntityTypeConfiguration<FileUpload>
{
    public void Configure(EntityTypeBuilder<FileUpload> builder)
    {
        builder.ToTable("FileUpload");
        builder.HasKey(f => f.Id);

    
        builder.Property(f => f.Name)
        .HasColumnName("name")
        .HasColumnType("varchar")
        .IsRequired()
        .HasMaxLength(250);

        builder.Property(f => f.Extension)
        .HasColumnName("extension")
        .HasColumnType("varchar")
        .IsRequired()
        .HasMaxLength(10);

        builder.Property(f => f.Size)
        .HasColumnName("size")
        .HasColumnType("double")
        .IsRequired();
        
        builder.Property(f => f.Route)
        .HasColumnName("route")
        .HasColumnType("varchar")
        .IsRequired()
        .HasMaxLength(250);



    }
}

