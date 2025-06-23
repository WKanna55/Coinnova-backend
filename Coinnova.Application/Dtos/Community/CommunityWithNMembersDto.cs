namespace Coinnova.Application.Dtos.Community;

public class CommunityWithNMembersDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    
    public int MemberCount { get; set; }
    public string ImageUrl { get; set; }
}