using API.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
    [HttpGet]
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
    public async Task<ActionResult<FileUploadDto>> Get(int id)
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
    public async Task<ActionResult<FileUploadDto>> PostFile([FromForm] FileUploadDto file)
    {
        var entidad = this.unitOfWork.FileUploads.PostFile(file);
        try
        {
            if (file != null)
            {
                
                var filePath = "C:\\Users\\APT01-38\\Desktop\\DG\\DOTNET\\UploadFiles\\BackEnd\\ProyUpload\\BackEnd\\Persistence\\Data\\Files\\img\\" + file.Name;
                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyToAsync(stream);
                }
                double size = file.Size;
                size = size / 1000000;
                size = Math.Round(size, 2);
                FileUpload fileUpload = new FileUpload();
                fileUpload.Extension = Path.GetExtension(file.FileName).Substring(1);
                fileUpload.Name = Path.GetFileNameWithoutExtension(file.FileName);
                fileUpload.Size = size;
                fileUpload.Route = filePath;
                unitOfWork.FileUploads.PostFile(fileUpload);
                }
            _context.SaveAsync();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(file);
    }
}