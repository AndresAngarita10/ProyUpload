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
    public async Task<ActionResult<FileUploadDto>> PostFile([FromForm] FileUpload file)
    {
        try
        {
            if (file == null)
            {
                return BadRequest("No se proporcionó un archivo válido.");
            }

            var entidad = await this.unitOfWork.FileUploads.PostFile(file);
            var entidadDto = new FileUploadDto
            {
                Id = entidad.Id,
                Name = entidad.Name,
                Extension = entidad.Extension,
                Size = entidad.Size,
                Route = entidad.Route
            };

            return CreatedAtAction(nameof(GetFile), new { id = entidad.Id }, entidadDto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}