using Coinnova.Domain.Common.Models;
using Microsoft.AspNetCore.Http;

namespace Coinnova.Application.Common.Files;

/*
 * Se agrego esto para que maneje using Microsoft.AspNetCore.Http;
 * esta libreria no deberia usarse aca puesto que es para endpoints(capa presentacion) -> maneja formularios
 * (subida de archivos),
 * pero priorizé que el controlador se vea limpio y que el servicio un poco más sucio
 */
public class FileUploadFactory
{
    /*
     * Completa un FileUpload (Dto especial para subida de archivos)
     */
    public async Task<FileUpload?> FromFormFileAsync(IFormFile file, string folder)
    {
        if (file == null || file.Length == 0)
            return null;

        var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        stream.Position = 0;

        return new FileUpload
        {
            FileName = file.FileName,
            Content = stream,
            ContentType = file.ContentType,
            Folder = folder
        };
    }
}