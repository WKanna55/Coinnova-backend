namespace Coinnova.Domain.Entities;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<CommunityCategory> CommunityCategory { get; set; } = new List<CommunityCategory>();

    public virtual ICollection<EventCategory> EventCategory { get; set; } = new List<EventCategory>();
}
