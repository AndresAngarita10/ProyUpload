
using Aplicacion.Repository;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repository;

public class FileUploadRepository: GenericRepository<FileUpload>, IFileUpload
{
    protected readonly ApiContext _context;
    
    public FileUploadRepository(ApiContext context) : base (context)
    {
        _context = context;
    }

    public override async Task<IEnumerable<FileUpload>> GetAllAsync()
    {
        return await _context.FileUploads
            .ToListAsync();
    }

    public override async Task<FileUpload> GetByIdAsync(int id)
    {
        return await _context.FileUploads
        .FirstOrDefaultAsync(p =>  p.Id == id);
    }
}
