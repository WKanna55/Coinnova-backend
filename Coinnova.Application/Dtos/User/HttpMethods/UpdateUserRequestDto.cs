using System.ComponentModel.DataAnnotations;

namespace Coinnova.Application.Dtos.User;

public class UpdateUserRequestDto
{
    public required string Name { get; set; }
    public required string Biography { get; set; }
    public required string ImageUrl { get; set; }
}