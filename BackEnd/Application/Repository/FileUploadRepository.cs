
using Aplicacion.Repository;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.IO;
using System.IO.Compression;
using System.Drawing;
using System.Drawing.Imaging;

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
        .FirstAsync(p => p.Id == id);
    }

    
    public async Task<IEnumerable<FileUpload>> GetAllDataTypeFiles(int type)
    {
        return await _context.FileUploads
            .Where(f => f.TypeFileFk == 2)
            .ToListAsync();
    }
    
    
    public  async Task<IEnumerable<FileUpload>> GetAllAsyncByType(string type)
    {
        int TypeFileFk = 0;
        if (type == "Image"){
            TypeFileFk = 1;
        }else if (type == "Document"){
            TypeFileFk = 2;
        }
        return await _context.FileUploads
            .Where(f => f.TypeFileFk == TypeFileFk)
            .ToListAsync();
    }

    
    public  async Task<FileUpload> GetFilenById(int id)
    {
        return await _context.FileUploads
            .Where(f => f.Id == id)
            .FirstAsync();
    }


    public async Task<FileUpload> FileUploadAsync(IFormFile file)
    {
        if (file == null || string.IsNullOrEmpty(file.FileName))
        {
            // Manejar el caso en el que file o file.FileName sean nulos
            return null;
        }
        string[] validExtensionsImg = { ".jpg", ".jpeg", ".png", ".gif", ".webp"  };
        string[] validDocumentExtensions = { ".pdf", ".doc", ".docx", ".txt", ".xlsx" };
        var extension = Path.GetExtension(file.FileName).ToLower();
        var filePath = "";
        int type = 0;
        if (validExtensionsImg.Contains(extension))
        {
            type = 1;
            string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            filePath = Path.Combine(userFolder, "Escritorio", "ProyUpload", "BackEnd", "Persistence", "Data", "Archivos", "Img", file.FileName);
            Console.WriteLine("esste es el log de userfolder " + userFolder);
            filePath = "C:\\Users\\andre\\OneDrive\\Escritorio\\ProyUpload\\BackEnd\\Persistence\\Data\\Archivos\\Img\\" + file.FileName;
            // Comprimir imágenes
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var image = Image.FromStream(memoryStream);
                var quality = 50; // Ajusta la calidad de compresión según tus necesidades
                var encoder = ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid);
                var encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);
                image.Save(filePath, encoder, encoderParameters);
            }
        }
        else if (validDocumentExtensions.Contains(extension))
        {
            type = 2;
            string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            filePath = Path.Combine(userFolder, "Escritorio", "ProyUpload", "BackEnd", "Persistence", "Data", "Archivos", "Doc", file.FileName);

            filePath = "C:\\Users\\andre\\OneDrive\\Escritorio\\ProyUpload\\BackEnd\\Persistence\\Data\\Archivos\\Doc\\" + file.FileName;
            using (var stream = File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }
        }
        else
        {
            return null;
        }
        double size = file.Length; //1.048.576 
        size = size / 1048576;
        size = Math.Round(size, 2);

        FileUpload fileUpload = new FileUpload
        {
            Extension = Path.GetExtension(file.FileName).Substring(1),
            Name = Path.GetFileNameWithoutExtension(file.FileName),
            Size = size,
            Route = filePath,
            TypeFileFk = type
        };

        return fileUpload;
    }
}
