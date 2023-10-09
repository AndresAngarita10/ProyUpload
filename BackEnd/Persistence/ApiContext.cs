
using System.Reflection;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class ApiContext : DbContext
{
    public ApiContext(DbContextOptions options) : base(options)
    { }
    public DbSet<FileUpload> FileUploads { get; set; }
    public DbSet<TypeFile> TypeFiles { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    }

}
