
using Aplicacion.Repository;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repository;

public class FileUploadRepository : GenericRepository<FileUpload>, IFileUpload
{
    protected readonly ApiContext _context;

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

    public async Task<FileUpload> FileUploadAsync(IFormFile file)
    {
        string[] validExtensionsImg = { ".jpg", ".jpeg", ".png", ".gif" };
        string[] validDocumentExtensions = { ".pdf", ".doc", ".docx", ".txt", ".xlsx" };
        var extension = Path.GetExtension(file.FileName).ToLower();
        var filePath = "";
        if (validExtensionsImg.Contains(extension))
        {
            filePath = "C:\\Users\\APT01-042\\Desktop\\ProyUpload\\BackEnd\\Persistence\\Data\\Archivos\\Imagenes\\" + file.FileName;
        }
        else if (validDocumentExtensions.Contains(extension))
        {
            filePath = "C:\\Users\\APT01-042\\Desktop\\ProyUpload\\BackEnd\\Persistence\\Data\\Archivos\\Documento\\" + file.FileName;
        }
        else
        {
            return null;
        }
        using (var stream = System.IO.File.Create(filePath))
        {
            await file.CopyToAsync(stream);
        }
        double size = file.Length;/* 1.048.576 */
        size = size / 1048576;
        size = Math.Round(size, 2);
        FileUpload fileUpload = new();
        fileUpload.Extension = Path.GetExtension(file.FileName).Substring(1);
        fileUpload.Name = Path.GetFileNameWithoutExtension(file.FileName);
        fileUpload.Size = size;
        fileUpload.Route = filePath;

        return fileUpload;
    }
}
