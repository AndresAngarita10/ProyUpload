
using API.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.IO;  // Para trabajar con archivos y flujos de memoria
using System.Drawing;  // Para trabajar con imágenes (si deseas convertir los datos de la imagen a un objeto de imagen)

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

    [HttpGet("Imagenes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<IEnumerable<Image/* byte[] */>>> Get11()
    {
        var files = await unitofwork.FileUploads.GetAllAsync();
        List<byte[]> byteList = new List<byte[]>();
        List<Image> imageList = new List<Image>();

        foreach (var file in files)
        {
            var filePath = file.Route;
            // Verifica si el archivo de origen existe
            if (System.IO.File.Exists(filePath))
            {
                byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
                // Ahora, 'imageBytes' contiene los datos de la imagen desde la ruta especificada.
                // Puedes retornar 'imageBytes' o convertirlo en una imagen u objeto adecuado según tus necesidades.
                byteList.Add(imageBytes);
                // Para convertir 'imageBytes' a una imagen, puedes usar algo como esto:
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    Image image = Image.FromStream(ms);
                    // Ahora, 'image' contiene la imagen cargada desde 'imageBytes'.
                    imageList.Add(image);
                }
            }
            else
            {
                // Manejar el caso en el que el archivo no existe.
                // Puedes lanzar una excepción, mostrar un mensaje de error, etc.
            }
        }
        /* return byteList; */
        return imageList;

    }

    [HttpGet("Imagenes2")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<FileResult>>> GetImages()
    {
        var files = await unitofwork.FileUploads.GetAllAsync();
        List<FileContentResult> imageResults = new List<FileContentResult>();

        foreach (var file in files)
        {
            var filePath = file.Route;
            if (System.IO.File.Exists(filePath))
            {
                byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);

                // Configura la respuesta como un archivo de imagen
                var imageResult = new FileContentResult(imageBytes, "image/jpeg"); // Reemplaza con el tipo de contenido adecuado



                Console.WriteLine("Foto : " + file.Extension);
                imageResults.Add(imageResult);
            }
            else
            {
                // Manejar el caso en el que el archivo no existe.
                // Puedes lanzar una excepción, mostrar un mensaje de error, etc.
            }
        }

        return imageResults;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IFormFile>> PostArchivos(IFormFile file)
    {

        string[] validExtensionsImg = { ".jpg", ".jpeg", ".png", ".gif" };
        string[] validDocumentExtensions = { ".pdf", ".doc", ".docx", ".txt", ".xlsx" };
        var extension = Path.GetExtension(file.FileName).ToLower();
        var filePath = "";
        if (validExtensionsImg.Contains(extension))
        {
            filePath = "C:\\Users\\APT01-042\\Desktop\\ProyUpload\\BackEnd\\Persistence\\Data\\Archivos\\Imagenes\\" + file.FileName;
        }
        else if (validDocumentExtensions.Contains(extension))
        {
            filePath = "C:\\Users\\APT01-042\\Desktop\\ProyUpload\\BackEnd\\Persistence\\Data\\Archivos\\Documento\\" + file.FileName;
        }
        else
        {
            return null;
        }
        using (var stream = System.IO.File.Create(filePath))
        {
            await file.CopyToAsync(stream);
        }
        double size = file.Length;/* 1.048.576 */
        size = size / 1048576;
        size = Math.Round(size, 2);
        FileUploadDto fileUploadDto = new();
        fileUploadDto.Extension = Path.GetExtension(file.FileName).Substring(1);
        fileUploadDto.Name = Path.GetFileNameWithoutExtension(file.FileName);
        fileUploadDto.Size = size;
        fileUploadDto.Route = filePath;

        var fileUpload = this.mapper.Map<FileUpload>(fileUploadDto);
        this.unitofwork.FileUploads.Add(fileUpload);
        await unitofwork.SaveAsync();
        if (fileUpload == null)
        {
            return BadRequest();
        }
        fileUploadDto.Id = fileUpload.Id;
        return CreatedAtAction(nameof(PostArchivos), new { id = fileUploadDto.Id }, fileUploadDto);

    }


    /* C:\Users\APT01-042\Desktop\ProyUpload\BackEnd\Persistence\Data\Archivos\Imagenes */

    /* [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FileUpload>> Post(FileUploadDto fileUploadDto)
    {
        var fileUpload = this.mapper.Map<FileUpload>(fileUploadDto);
        this.unitofwork.FileUploads.Add(fileUpload);
        await unitofwork.SaveAsync();
        if(fileUpload == null)
        {
            return BadRequest();
        }
        fileUploadDto.Id = fileUpload.Id;
        return CreatedAtAction(nameof(Post), new {id = fileUploadDto.Id}, fileUploadDto);
    } */
}
