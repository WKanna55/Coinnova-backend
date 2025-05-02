namespace Coinnova.Domain.Entities;

public partial class Comment
{
    public int Id { get; set; }

    public string Content { get; set; } = null!;

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public int? Likes { get; set; }

    public int? IdType { get; set; }

    public int IdUser { get; set; }

    public int IdPost { get; set; }

    public int? IdParentComment { get; set; }

    public virtual Comment? IdParentCommentNavigation { get; set; }

    public virtual Post IdPostNavigation { get; set; } = null!;

    public virtual CommentType? IdTypeNavigation { get; set; }

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual ICollection<Comment> InverseIdParentCommentNavigation { get; set; } = new List<Comment>();
}
