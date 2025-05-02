namespace Coinnova.Domain.Entities;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<User> User { get; set; } = new List<User>();
}
