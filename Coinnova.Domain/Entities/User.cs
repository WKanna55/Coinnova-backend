namespace Coinnova.Domain.Entities;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Biography { get; set; }

    public string? Imageurl { get; set; }

    public int IdRole { get; set; }

    public DateTime? Createdat { get; set; }

    public int? IdInstitution { get; set; }
    public string AuthProvider { get; set; } = "Local";
    public virtual ICollection<Chat> ChatIdUser1Navigation { get; set; } = new List<Chat>();

    public virtual ICollection<Chat> ChatIdUser2Navigation { get; set; } = new List<Chat>();

    public virtual ICollection<Comment> Comment { get; set; } = new List<Comment>();

    public virtual ICollection<CommunityMember> CommunityMember { get; set; } = new List<CommunityMember>();

    public virtual ICollection<Event> Event { get; set; } = new List<Event>();

    public virtual Institution? IdInstitutionNavigation { get; set; }

    public virtual Role IdRoleNavigation { get; set; } = null!;

    public virtual ICollection<Message> Message { get; set; } = new List<Message>();

    public virtual ICollection<Notification> Notification { get; set; } = new List<Notification>();

    public virtual ICollection<Post> Post { get; set; } = new List<Post>();
}
