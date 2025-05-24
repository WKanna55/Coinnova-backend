using Microsoft.AspNetCore.Http;

namespace Coinnova.Application.Dtos.Post;

public class PostPostDto
{
    public string Title { get; set; } = null!;
    public string Textcontent { get; set; } = null!;
    public int IdType { get; set; }
    public int IdUser { get; set; }
    public int IdCommunity { get; set; }
    public IFormFile? File { get; set; }  // Importante: esto necesita [FromForm]
}