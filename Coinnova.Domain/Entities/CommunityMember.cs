namespace Coinnova.Domain.Entities;

public partial class CommunityMember
{
    public int Id { get; set; }

    public int IdUser { get; set; }

    public int IdCommunity { get; set; }

    public DateTime? Joinedat { get; set; }

    public virtual Community IdCommunityNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
