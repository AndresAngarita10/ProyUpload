
using Aplicacion.Repository;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.IO;
namespace Application.Repository;

public class FileUploadRepository : GenericRepository<FileUpload>, IFileUpload
{
    protected readonly ApiContext _context;
    private readonly IUnitOfWork unitOfWork;

    public FileUploadRepository(ApiContext context) : base(context)
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
        .FirstOrDefaultAsync(p => p.Id == id);
    }

    /*
    public async Task<FileUpload> PostFile(FileUpload fileUpload)
    {
        var filePath = Path.Combine("C:\\Users\\APT01-38\\Desktop\\DG\\DOTNET\\UploadFiles\\BackEnd\\ProyUpload\\BackEnd\\Persistence\\Data\\Files\\img\\", fileUpload.Name + "." + fileUpload.Extension);
        using (var stream = File.Create(filePath))
        {
            await unitOfWork.fileUpload.CopyToAsync(stream);
        }
        double size = fileUpload.Length;
        size = size / 1000000;
        size = Math.Round(size, 2);
        fileUpload.Size = size;
        fileUpload.Route = filePath;
        unitOfWork.FileUploads.PostFile(fileUpload);
        _context.FileUploads.Add(fileUpload);
        return fileUpload;
    }
    */
    public async Task<FileUpload> AddFile(FileUpload file)
    {
        var safe = await _context.FileUploads.AddAsync(file);
        await _context.SaveChangesAsync();
        return safe.Entity;
    }
}
