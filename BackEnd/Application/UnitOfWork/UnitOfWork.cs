using Application.Repository;
using Domain.Interfaces;
using Persistence;

namespace Aplicacion.UnitOfWork;
public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly ApiContext _context;
    private FileUploadRepository _files;

    public UnitOfWork(ApiContext context)
    {
        _context = context;
    }
    public IFileUpload FileUploads
    {
        get
        {
            if (_files == null)
            {
                _files = new FileUploadRepository(_context);
            }
            return _files;
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }
    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
