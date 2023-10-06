
using Domain.Entities;
using Dominio.Interfaces;

namespace Domain.Interfaces;

public interface IFileUpload : IGenericRepo<FileUpload>
{
    Task<IEnumerable<FileUpload>> PostFile();
}
