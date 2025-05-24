namespace Coinnova.Domain.Common.Models;

/*
 * Dto especial, mas como una configuracion, hecho para la subida de archivos
 */
public class FileUpload
{
    public string FileName { get; set; } = string.Empty;
    public Stream Content { get; set; } = default!;
    public string ContentType { get; set; } = string.Empty;
    public string Folder { get; set; } = string.Empty;
}