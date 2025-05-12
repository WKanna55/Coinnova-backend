namespace Coinnova.Application.Dtos.Category;

public class CommunityWithMembersDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Members { get; set; }
}