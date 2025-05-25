using Microsoft.AspNetCore.Http;

namespace Coinnova.Application.Dtos.Event;

public class CreateEventDto
{
    public DateTime Initialdate { get; set; }

    public DateTime Enddate { get; set; }

    public string? Place { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? Createdby { get; set; }

    public bool VisibilityPrivate { get; set; }
    
    public IFormFile? Image { get; set; }  // Importante: esto necesita [FromForm]
    
    public IFormFile? File { get; set; }  // Importante: esto necesita [FromForm]
    
}