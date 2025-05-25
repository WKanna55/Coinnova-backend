using Microsoft.AspNetCore.Http;

namespace Coinnova.Application.Dtos.Event;

public class UploadEventImageDto
{
    public int EventId { get; set; }
    
    public IFormFile? Image { get; set; }  // Importante: esto necesita [FromForm]
}