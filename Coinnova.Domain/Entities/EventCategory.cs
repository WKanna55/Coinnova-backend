namespace Coinnova.Domain.Entities;

public partial class EventCategory
{
    public int Id { get; set; }

    public int? IdEvent { get; set; }

    public int? IdCategory { get; set; }

    public virtual Category? IdCategoryNavigation { get; set; }

    public virtual Event? IdEventNavigation { get; set; }
}
