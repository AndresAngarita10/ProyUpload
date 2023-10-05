
using Domain.Entities;

namespace API.Dtos;

public class FileUploadDto : BaseEntity
{
    public string Name { get; set;}
    public string Extension { get; set; }
    public double Size { get; set; }
    public string Route { get; set; }
}
