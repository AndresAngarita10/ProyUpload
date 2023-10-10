using API.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.IO;  // Para trabajar con archivos y flujos de memoria
using System.Drawing;  // Para trabajar con imágenes (si deseas convertir los datos de la imagen a un objeto de imagen)
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

using SixLabors.ImageSharp.Formats.Jpeg;
namespace API.Controllers;

public class FileUploadController : BaseApiController
{
    private readonly IUnitOfWork unitofwork;
    private readonly IMapper mapper;

    public FileUploadController(IUnitOfWork unitofwork, IMapper mapper)
    {
        this.unitofwork = unitofwork;
        this.mapper = mapper;
    }


    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<IEnumerable<FileUploadDto>>> Get()
    {
        var files = await unitofwork.FileUploads.GetAllAsync();
        return mapper.Map<List<FileUploadDto>>(files);
    }
}
