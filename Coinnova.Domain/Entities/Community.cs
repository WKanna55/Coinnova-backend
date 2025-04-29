namespace Coinnova.Domain.Entities;

public partial class Community
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? Createdat { get; set; }

    public string? Imageurl { get; set; }

    public int? IdInstitution { get; set; }

    public virtual ICollection<CommunityCategory> CommunityCategory { get; set; } = new List<CommunityCategory>();

    public virtual ICollection<CommunityMember> CommunityMember { get; set; } = new List<CommunityMember>();

    public virtual Institution? IdInstitutionNavigation { get; set; }

    public virtual ICollection<Post> Post { get; set; } = new List<Post>();
}
