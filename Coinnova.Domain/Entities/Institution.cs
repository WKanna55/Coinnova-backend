namespace Coinnova.Domain.Entities;

public partial class Institution
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Domain { get; set; }

    public string? Imageurl { get; set; }

    public virtual ICollection<Community> Community { get; set; } = new List<Community>();

    public virtual ICollection<InstitutionEvent> InstitutionEvent { get; set; } = new List<InstitutionEvent>();

    public virtual ICollection<User> User { get; set; } = new List<User>();
}
