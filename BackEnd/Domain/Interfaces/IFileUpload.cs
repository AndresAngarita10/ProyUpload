
using Domain.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Domain.Interfaces;

public interface IFileUpload : IGenericRepo<FileUpload>
{
    Task<FileUpload> PostFile(FileUpload file);
}
