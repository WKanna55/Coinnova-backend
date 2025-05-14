namespace Coinnova.Application.Dtos.Event;

public class EventPreviewDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public DateTime InitialDate { get; set; }
    public string SourceName { get; set; } // Nombre de la institución o categoría
}