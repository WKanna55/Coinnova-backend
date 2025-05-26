namespace Coinnova.Application.Dtos.Event;

public class EventDto
{
    public int Id { get; set; }

    public DateTime Initialdate { get; set; }

    public DateTime Enddate { get; set; }

    public string? Place { get; set; }

    public string Name { get; set; } = null!;

    public string? Imageurl { get; set; }

    public string? Rulesurl { get; set; }

    public string? Description { get; set; }

    public int? Createdby { get; set; }
    
    public bool VisibilityPrivate { get; set; }
}