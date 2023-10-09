
namespace Domain.Entities;

public class FileUpload : BaseEntity
{
    public string Name { get; set; }
    public string Extension { get; set; }
    public double Size { get; set; }
    public string Route { get; set; }
    public int TypeFileFk { get; set; }
    public TypeFile TypeFile { get; set; }

}
