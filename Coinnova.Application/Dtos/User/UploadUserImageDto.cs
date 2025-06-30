using Microsoft.AspNetCore.Http;

namespace Coinnova.Application.Dtos.User;

public class UploadUserImageDto
{
    public int UserId { get; set; }
    public IFormFile? Image { get; set; } // Importante: esto necesita [FromForm]
}