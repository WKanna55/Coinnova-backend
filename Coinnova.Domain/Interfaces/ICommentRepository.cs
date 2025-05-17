using Coinnova.Domain.Entities;
using Coinnova.Domain.Interfaces.Base;

namespace Coinnova.Domain.Interfaces;

public interface ICommentRepository : IRepository<Comment>
{
    
    /// <summary>
    /// Obtiene los comentarios raíz de un post, junto con el conteo de sus respuestas directas.
    /// </summary>
    /// <param name="postId">El ID del post.</param>
    /// <returns>Una colección de tuplas (ComentarioRaiz, ConteoDeSusRespuestas).</returns>
    Task<IEnumerable<(Comment Comment, int ReplyCount)>> GetRootCommentsWithReplyCountAsync(int postId);
    Task<IEnumerable<(Comment Comment, int ReplyCount)>> GetRepliesWithReplyCountAsync(int parentCommentId);
    Task<int> CountRepliesByCommentIdAsync(int commentId);
    //Task<IEnumerable<Comment>> GetAllRootCommentsByPostId(int postId);
}