using Microsoft.AspNetCore.Http;

namespace Coinnova.Application.Dtos.Post;

public class UploadPostImageDto
{
    public int PostId { get; set; }
    public IFormFile? File { get; set; }  // Importante: esto necesita [FromForm]
}