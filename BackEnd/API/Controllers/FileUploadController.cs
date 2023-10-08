using AutoMapper;
using Microsoft.AspNetCore.Mvc;

using Domain.Entities;
using Domain.Interfaces;
using API.Dtos;

namespace API.Controllers;
public class FileUploadController : BaseApiController
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    public FileUploadController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }
    [HttpGet("GetFiles")]
    public async Task<ActionResult<IEnumerable<FileUploadDto>>> Get()
    {
        var entidad = await unitOfWork.FileUploads.GetAllAsync();
        try
        {
            return Ok(mapper.Map<List<FileUploadDto>>(entidad));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("GetFileById/{id}")]
    public async Task<ActionResult<FileUploadDto>> GetById(int id)
    {
        var entidad = await unitOfWork.FileUploads.GetByIdAsync(id);
        try
        {
            return Ok(mapper.Map<FileUploadDto>(entidad));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FileUploadDto>> GetFile(int id)
    {
        var entidad = await unitOfWork.FileUploads.GetByIdAsync(id);
        if (entidad == null)
        {
            return NotFound();
        }
        return this.mapper.Map<FileUploadDto>(entidad);
    }

    [HttpPost("PostFile")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FileUpload>> PostFile(IFormFile file)
    {
        try
        {
            var filePath = "D:\\Users\\dalgr\\Documents\\Campus\\Ciclo3\\NetCore\\ProyectoUploadFile\\ProyUpload\\BackEnd\\Persistence\\Data\\Files\\img\\" + Path.GetFileName(file.FileName);
            /* var filePath = "C:\\Users\\APT01-38\\Desktop\\DG\\DOTNET\\UploadFiles\\BackEnd\\ProyUpload\\BackEnd\\Persistence\\Data\\Files\\img\\" + Path.GetFileName(file.FileName); */
            using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }
            double size = file.Length / 1000000.0;
            size = Math.Round(size, 2);
            FileUpload upload = new FileUpload();
            upload.Extension = Path.GetExtension(file.FileName).Substring(1);
            upload.Name = Path.GetFileNameWithoutExtension(file.FileName);
            upload.Size = size;
            upload.Route = filePath;
            await unitOfWork.FileUploads.AddFile(upload);
            return CreatedAtAction(nameof(GetFile), new { id = upload.Id }, upload);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("PostImg")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FileUpload>> PostImg(IFormFile file)
    {
        try
        {
            if (file == null /* ||  (file.Length > 0 && file.Length <= 200000000) */)
            {
                return BadRequest("No se proporcionó un archivo válido o no cumple con peso permitido");
            }
            else
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif",".tiff",".svg"};
                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest("Solo se permiten archivos de imagen (jpg, jpeg, png, gif).");
                }
                var filePath = Path.Combine(
                    "D:\\Users\\dalgr\\Documents\\Campus\\Ciclo3\\NetCore\\ProyectoUploadFile\\ProyUpload\\BackEnd\\Persistence\\Data\\Files\\img\\",
                    /*"C:\\Users\\APT01-38\\Desktop\\DG\\DOTNET\\UploadFiles\\BackEnd\\ProyUpload\\BackEnd\\Persistence\\Data\\Files\\img\\",*/
                    Path.GetFileName(file.FileName));
                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }
                double size = file.Length / 1000000.0;
                size = Math.Round(size, 2);
                FileUpload upload = new FileUpload
                {
                    Extension = Path.GetExtension(file.FileName).Substring(1),
                    Name = Path.GetFileNameWithoutExtension(file.FileName),
                    Size = size,
                    Route = filePath
                };
                await unitOfWork.FileUploads.AddFile(upload);
                return CreatedAtAction(nameof(GetFile), new { id = upload.Id }, upload);
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}