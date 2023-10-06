namespace API.Dtos;
public class FileUploadDto
{
    public int Id { get; set;}
    public string Name { get; set;}
    public string Extension { get; set; }
    public double Size { get; set; }
    public string Route { get; set; }
    public IFormFile Sfile { get; set; }
}