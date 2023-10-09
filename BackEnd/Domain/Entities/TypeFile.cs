
namespace Domain.Entities;

public class TypeFile : BaseEntity
{
    public string Description { get; set; }
    public ICollection<FileUpload> FileUploads { get; set; }
}
