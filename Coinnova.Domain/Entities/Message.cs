namespace Coinnova.Domain.Entities;

public partial class Message
{
    public int Id { get; set; }

    public string Content { get; set; } = null!;

    public int IdSender { get; set; }

    public int IdChat { get; set; }

    public DateTime? Date { get; set; }

    public virtual Chat IdChatNavigation { get; set; } = null!;

    public virtual User IdSenderNavigation { get; set; } = null!;
}
