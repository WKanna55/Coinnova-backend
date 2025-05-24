using Coinnova.Application.Dtos.Community;
using Coinnova.Application.Dtos.User;

namespace Coinnova.Application.Dtos.Post;

/*
 * No usar más
 * Usar herencia en este contexto viola el principio de responsabilidad única (SRP):
 * cada DTO debe ser autónomo y específico para su uso.
 * Complica el uso de AutoMapper o validaciones
 * Se prefiere mantener los DTOs separados y específicos.
 */

public class BasePostDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string TextContent { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int Likes { get; set; }
    public required string PostTypeName { get; set; }
    public required string ImageUrl { get; set; }
    public int CommentCount { get; set; }
    public required UserSimpleDto Author { get; set; }
    public required CommunityBaseDto Community { get; set; }
}


