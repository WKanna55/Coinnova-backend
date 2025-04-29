namespace Coinnova.Domain.Entities;

public partial class InstitutionEvent
{
    public int Id { get; set; }

    public int? IdInstitution { get; set; }

    public int? IdEvent { get; set; }

    public virtual Event? IdEventNavigation { get; set; }

    public virtual Institution? IdInstitutionNavigation { get; set; }
}
