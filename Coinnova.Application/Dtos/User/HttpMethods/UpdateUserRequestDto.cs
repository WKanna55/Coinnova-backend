using Microsoft.AspNetCore.Http;

namespace Coinnova.Application.Dtos.User.HttpMethods;

public class UpdateUserRequestDto
{
    public string? Name { get; set; }
    public string? Biography { get; set; }
    public IFormFile? Image { get; set; }  // Importante: esto necesita [FromForm]
}