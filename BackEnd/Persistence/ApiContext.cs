
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class ApiContext : DbContext
{
    public ApiContext(DbContextOptions options) : base(options)
    { }
    public DbSet<FileUpload> FileUploads { get; set; }

}
