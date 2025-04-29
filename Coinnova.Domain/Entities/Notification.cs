namespace Coinnova.Domain.Entities;

public partial class Notification
{
    public int Id { get; set; }

    public string Content { get; set; } = null!;

    public int RefId { get; set; }

    public string Entity { get; set; } = null!;

    public DateTime? Date { get; set; }

    public int IdUser { get; set; }

    public virtual User IdUserNavigation { get; set; } = null!;
}
