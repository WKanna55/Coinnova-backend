using System.ComponentModel.DataAnnotations.Schema;

namespace Coinnova.Domain.Entities;

public partial class Post
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Textcontent { get; set; } = null!;

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public int? Likes { get; set; }

    public int IdType { get; set; }

    public string? Imageurl { get; set; }

    public int IdUser { get; set; }

    public int IdCommunity { get; set; }

    public virtual ICollection<Comment> Comment { get; set; } = new List<Comment>();

    public virtual Community IdCommunityNavigation { get; set; } = null!;

    public virtual PostType IdTypeNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;

    [NotMapped]
    public int CommentCount { get; set; }
}
