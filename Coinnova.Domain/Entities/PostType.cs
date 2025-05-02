namespace Coinnova.Domain.Entities;

public partial class PostType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Post> Post { get; set; } = new List<Post>();
}
