namespace Coinnova.Domain.Entities;

public partial class CommunityCategory
{
    public int Id { get; set; }

    public int? IdCommunity { get; set; }

    public int? IdCategory { get; set; }

    public virtual Category? IdCategoryNavigation { get; set; }

    public virtual Community? IdCommunityNavigation { get; set; }
}
