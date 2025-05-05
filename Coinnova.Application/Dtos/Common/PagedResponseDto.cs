namespace Coinnova.Application.Dtos.Common;

public class PagedResponseDto<T>
{
    public IEnumerable<T> Items { get; set; } = new List<T>();
    public bool HasMore { get; set; }
    public int TotalCount { get; set; }
}