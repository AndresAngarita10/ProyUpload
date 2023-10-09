
using Domain.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Domain.Interfaces;

public interface IFileUpload : IGenericRepo<FileUpload>
{
     public Task<FileUpload> FileUploadAsync(IFormFile file);
     public Task<IEnumerable<FileUpload>> GetAllAsyncByType(string type);
     public Task<FileUpload> GetFilenById(int id);
     public Task<IEnumerable<FileUpload>> GetAllDataTypeFiles(int type);
     
}
