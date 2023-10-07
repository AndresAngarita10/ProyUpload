using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.IO;

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

    /*[HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<FileUploadDto>>> Get()
    {
        var entidad = await unitOfWork.FileUploads.GetAllAsync();
        return mapper.Map<List<FileUploadDto>>(entidad);
    }*/


    /*[HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FileUpload>> Post(FileUploadDto entidadDto)
    {
        var entidad = this.mapper.Map<FileUpload>(entidadDto);
        this.unitOfWork.FileUploads.Add(entidad);
        await unitOfWork.SaveAsync();
        if(entidad == null)
        {
            return BadRequest();
        }
        entidadDto.Id = entidad.Id;
        return CreatedAtAction(nameof(Post), new {id = entidadDto.Id}, entidadDto);
    }*/
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
[HttpPost]
[ProducesResponseType(StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
public async Task<ActionResult<FileUpload>> PostFile([FromForm] IFormFile file)
{
    try
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No se proporcionó un archivo válido.");
        }
        else
        {
            var filePath = "C:\\Users\\APT01-38\\Desktop\\DG\\DOTNET\\UploadFiles\\BackEnd\\ProyUpload\\BackEnd\\Persistence\\Data\\Files\\img\\" + Path.GetFileName(file.FileName);
            
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

            await unitOfWork.FileUploads.PostFile(upload);

            return CreatedAtAction(nameof(GetFile), new { id = upload.Id }, upload);
        }
    }
    catch (Exception ex)
    {
        return BadRequest(ex.Message);
    }
}

}