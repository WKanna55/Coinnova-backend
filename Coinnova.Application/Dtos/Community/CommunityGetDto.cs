namespace Coinnova.Application.Dtos.Community;

public class CommunityGetDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    
    public int NumberOfMembers { get; set; }
}