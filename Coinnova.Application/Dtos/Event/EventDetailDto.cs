namespace Coinnova.Application.Dtos.Event;

public class EventDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Place { get; set; }
    public string Description { get; set; }
    public DateTime InitialDate { get; set; }
    public DateTime EndDate { get; set; }
    public string RulesUrl { get; set; }
    
    public string ImageUrl { get; set; }
}