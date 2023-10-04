
namespace Domain.Entities;

public class FileUpload : BaseEntity
{
    public string Name { get; set;}
    public string Extension { get; set; }
    public double Size { get; set; }
    public string Route { get; set; }

}
