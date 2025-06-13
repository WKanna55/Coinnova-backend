namespace Coinnova.Application.Dtos.Comment;

public class CreateCommentDto
{
    public string Content { get; set; } = null!;
    public int? IdType { get; set; }  // Solo si es comentario de primer nivel
    public int IdUser { get; set; }
    public int IdPost { get; set; }
    public int? IdParentComment { get; set; }  // Solo si es respuesta a otro comentario
}