using Movie_Application.Models;

namespace Movie_Application.Repository.Interface
{
    public interface ICommentRepository
    {
        Task AddComment(Comment comment);
        Task<Comment> GetCommentById(Guid CommentId);
        Task<List<Comment>> GetMovieComments(Guid movieId);
        Task UpdateComment(Comment comment);
        Task DeleteComment(Guid commentId);

    }
}
