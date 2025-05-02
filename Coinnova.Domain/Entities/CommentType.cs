namespace Coinnova.Domain.Entities;

public partial class CommentType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Comment> Comment { get; set; } = new List<Comment>();
}
