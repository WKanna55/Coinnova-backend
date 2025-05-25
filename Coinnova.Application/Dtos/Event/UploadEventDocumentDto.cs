using Microsoft.AspNetCore.Http;

namespace Coinnova.Application.Dtos.Event;

public class UploadEventDocumentDto
{
    public int EventId { get; set; }
    
    public IFormFile? Document { get; set; }  // Importante: esto necesita [FromForm]
}