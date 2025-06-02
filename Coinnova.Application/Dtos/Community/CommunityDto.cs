namespace Coinnova.Application.Dtos.Community;

public class CommunityDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Imageurl { get; set; }
}