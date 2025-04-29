namespace Coinnova.Domain.Entities;

public partial class Chat
{
    public int Id { get; set; }

    public int IdUser1 { get; set; }

    public int IdUser2 { get; set; }

    public DateTime? Createdat { get; set; }

    public virtual User IdUser1Navigation { get; set; } = null!;

    public virtual User IdUser2Navigation { get; set; } = null!;

    public virtual ICollection<Message> Message { get; set; } = new List<Message>();
}
