
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

    [HttpGet("Imagenes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<IEnumerable<System.Drawing.Image/* byte[] */>>> Get11()
    {
        var files = await unitofwork.FileUploads.GetAllAsync();
        List<byte[]> byteList = new List<byte[]>();
        List<System.Drawing.Image> imageList = new List<System.Drawing.Image>();

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
                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
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

    [HttpGet("Imagenes2222")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<FileResult>>> GetImages2()
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
                /* var imageResult = new FileContentResult(imageBytes, "image/png"); */ // Reemplaza con el tipo de contenido adecuado


                // Convierte la imagen a base64
                string base64Image = Convert.ToBase64String(imageBytes);

                // Crea un objeto JSON con el campo filecontent
                var result = new { filecontent = $"data:image/png;base64,{base64Image}" };


                Console.WriteLine("Foto : " + file.Extension);
                /* imageResults.Add(result); */
            }
            else
            {
                // Manejar el caso en el que el archivo no existe.
                // Puedes lanzar una excepción, mostrar un mensaje de error, etc.
            }
        }

        return imageResults;
    }

    
    [HttpGet("Imagenes2")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<string>>> GetImages()
    {
        var files = await unitofwork.FileUploads.GetAllAsync();
        List<string> imageResults = new List<string>();

        foreach (var file in files)
        {
            var filePath = file.Route;
            if (System.IO.File.Exists(filePath))
            {
                byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);

                // Convierte la imagen a base64
                string base64Image = Convert.ToBase64String(imageBytes);

                Console.WriteLine("Foto : " + file.Extension);
                imageResults.Add($"data:image/png;base64,{base64Image}");
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
        if (file == null || file.Length == 0)
        {
            return BadRequest("El archivo está vacío.");
        }

        try
        {
            using (var image = SixLabors.ImageSharp.Image.Load(file.OpenReadStream()))
            {
                // Realiza una operación de redimensionamiento
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new SixLabors.ImageSharp.Size(200, 200),
                    Mode = ResizeMode.Max
                }));

                // Guarda la imagen redimensionada en el sistema de archivos
                string filePath = "C:\\Users\\APT01-042\\Desktop\\ProyUpload\\BackEnd\\Persistence\\Data\\Archivos\\Imagenes\\" + file.FileName; // Reemplaza con la ruta y nombre deseado
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    image.Save(stream, new JpegEncoder());
                }
            }

            FileUpload fileObject = await unitofwork.FileUploads.FileUploadAsync(file);
            var fileUpload = this.mapper.Map<FileUpload>(fileObject);
            this.unitofwork.FileUploads.Add(fileUpload);
            await unitofwork.SaveAsync();

            if (fileUpload == null)
            {
                return BadRequest();
            }

            fileObject.Id = fileUpload.Id;
            return CreatedAtAction(nameof(PostArchivos), new { id = fileObject.Id }, fileObject);
        }
        catch (Exception ex)
        {
            return BadRequest("Error al procesar la imagen: " + ex.Message);
        }
    }

    /*  [HttpPost]
     [ProducesResponseType(StatusCodes.Status201Created)]
     [ProducesResponseType(StatusCodes.Status400BadRequest)]
     public async Task<ActionResult<IFormFile>> PostArchivos(IFormFile file)
     {
         FileUpload fileObject = await unitofwork.FileUploads.FileUploadAsync(file);
         var fileUpload = this.mapper.Map<FileUpload>(fileObject);
         this.unitofwork.FileUploads.Add(fileUpload);
         await unitofwork.SaveAsync();
         if (fileUpload == null)
         {
             return BadRequest();
         }
         fileObject.Id = fileUpload.Id;
         return CreatedAtAction(nameof(PostArchivos), new { id = fileObject.Id }, fileObject);

     } */


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
