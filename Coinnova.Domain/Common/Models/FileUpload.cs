namespace Coinnova.Domain.Common.Models;

public class FileUpload
{
    public string FileName { get; set; } = string.Empty;
    public Stream Content { get; set; } = default!;
    public string ContentType { get; set; } = string.Empty;
    public string Folder { get; set; } = string.Empty;
}