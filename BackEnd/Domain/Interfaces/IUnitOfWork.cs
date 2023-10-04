
namespace Domain.Interfaces;

public interface IUnitOfWork
{
    IFileUpload FileUploads { get; }
    Task<int> SaveAsync();
}
