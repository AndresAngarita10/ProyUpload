
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Persistence;

public class ApiContext : DbContext
{
    public ApiContext(DbContextOptions options) : base(options)
    { }
    public DbSet<FileUpload> FileUploads { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

}
