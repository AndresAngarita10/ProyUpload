
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

    [HttpGet("dataDocs")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<FileUploadDto>>> GetAllDataTypeFiles()
    {
        var files = await unitofwork.FileUploads.GetAllDataTypeFiles(2);
        return mapper.Map<List<FileUploadDto>>(files);
    }



    [HttpGet("Img")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<string>>> GetImages()
    {
        var files = await unitofwork.FileUploads.GetAllAsyncByType("Image");
        List<object> imageResults = new List<object>();
        var response = new { Image = "", id = 0, size = 0.0 };
        foreach (var file in files)
        {
            var filePath = file.Route;
            if (System.IO.File.Exists(filePath))
            {
                byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);

                // Convierte la imagen a base64
                string base64Image = Convert.ToBase64String(imageBytes);

                Console.WriteLine("Foto : " + file.Extension);
                //imageResults.Add($"data:image/png;base64,{base64Image}");

                response = new
                {
                    Image = $"data:image/png;base64,{base64Image}",
                    id = file.Id,
                    size = Convert.ToDouble(file.Size)
                };
                imageResults.Add(response);
            }
            else
            {
                return NoContent();
            }
        }

        //return imageResults;
        //Console.WriteLine(imageResults[0]);
        return new JsonResult(imageResults);
    }

    [HttpGet("file/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetFileById(int id)
    {
        var file = await unitofwork.FileUploads.GetByIdAsync(id);
        string image = "";
        var filePath = file.Route;
        if (System.IO.File.Exists(filePath))
        {
            if (file.TypeFileFk == 1)
            {
                byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);

                // Convierte la imagen a base64
                string base64Image = Convert.ToBase64String(imageBytes);
                image = $"data:image/jpg;base64,{base64Image}";
                Console.WriteLine("Foto : " + file.Extension);

                var response = new
                {
                    Image = image,
                    id = file.Id,
                    Name = file.Name
                };
                return new JsonResult(response);
            }
            if (file.TypeFileFk == 2)
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                // Determina el tipo MIME adecuado para el archivo (por ejemplo, "application/pdf" para un archivo PDF).
                string contentType = "application/" + file.Extension;

                // Establece el nombre de archivo que se mostrará al descargar el archivo.
                string fileName = file.Name + "." + file.Extension;
                Console.WriteLine(file.Extension);
                // Configura el encabezado 'content-disposition' en la respuesta.
                Response.Headers.Add("content-disposition", "attachment; filename=" + fileName);

                return File(fileBytes, contentType);
            }
        }
        else
        {
            return NoContent();
        }
        return NoContent();
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IFormFile>> PostFiles(IFormFile file)
    {
        FileUpload fileObject = await unitofwork.FileUploads.FileUploadAsync(file);
        if (fileObject == null)
        {
            return BadRequest();
        }
        var fileUpload = this.mapper.Map<FileUpload>(fileObject);
        this.unitofwork.FileUploads.Add(fileUpload);
        await unitofwork.SaveAsync();
        if (fileUpload == null)
        {
            return BadRequest();
        }
        fileObject.Id = fileUpload.Id;
        return CreatedAtAction(nameof(PostFiles), new { id = fileObject.Id }, fileObject);

    }


    [HttpGet("Doc")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetDocs()
    {
        var files = await unitofwork.FileUploads.GetAllAsyncByType("Document");

        foreach (var file in files)
        {
            var filePath = file.Route;

            if (System.IO.File.Exists(filePath))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                // Determine el tipo MIME adecuado para el archivo (ejemplo: application/pdf para PDF).
                string contentType = "application/" + file.Extension; 

                
                string fileName = file.Name;

                return File(fileBytes, contentType, fileName);
            }
            else
            {
                
            }
        }

        
        return NotFound();
    }













    /* 
    [HttpGet("Doc")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<string>>> GetDocs()
    {
        var files = await unitofwork.FileUploads.GetAllAsyncByType("Document");
        List<string> docResult = new List<string>();

        foreach (var file in files)
        {
            var filePath = file.Route;
            if (System.IO.File.Exists(filePath))
            {
                byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);

                // Convierte la imagen a base64
                string base64Image = Convert.ToBase64String(imageBytes);

                Console.WriteLine("Foto : " + file.Extension);
                docResult.Add($"data:image/png;base64,{base64Image}");
            }
            else
            {
                // Manejar el caso en el que el archivo no existe.
                // Puedes lanzar una excepción, mostrar un mensaje de error, etc.
            }
        }

        return docResult;
    } */


    /* 
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
                    //string filePath = "C:\\Users\\APT01-042\\Desktop\\ProyUpload\\BackEnd\\Persistence\\Data\\Archivos\\Imagenes\\" + file.FileName;
                    string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                    string filePath = Path.Combine(userFolder, "Desktop", "ProyUpload", "BackEnd", "Persistence", "Data", "Archivos", "Imagenes", file.FileName);
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

    /*  [HttpGet("Imagenes2222")]
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
                 // var imageResult = new FileContentResult(imageBytes, "image/png");  // Reemplaza con el tipo de contenido adecuado


                 // Convierte la imagen a base64
                 string base64Image = Convert.ToBase64String(imageBytes);

                 // Crea un objeto JSON con el campo filecontent
                 var result = new { filecontent = $"data:image/png;base64,{base64Image}" };


                 Console.WriteLine("Foto : " + file.Extension);
                 // imageResults.Add(result); 
             }
             else
             {
                 // Manejar el caso en el que el archivo no existe.
                 // Puedes lanzar una excepción, mostrar un mensaje de error, etc.
             }
         }

         return imageResults;
     } */
}
