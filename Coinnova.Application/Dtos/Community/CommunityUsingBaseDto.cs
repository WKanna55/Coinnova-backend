namespace Coinnova.Application.Dtos.Community;

public class CommunityUsingBaseDto : CommunityBaseDto
{
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public int MemberCount { get; set; }
}