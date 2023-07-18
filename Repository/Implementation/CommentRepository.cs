using Microsoft.EntityFrameworkCore;
using Movie_Application.Data;
using Movie_Application.Models;
using Movie_Application.Repository.Interface;

namespace Movie_Application.Repository.Implementation
{
    public class CommentRepository : ICommentRepository
    {
        private readonly MovieContext _context;

        public CommentRepository(MovieContext contex)
        {
            _context = contex;
        }
        public async Task AddComment(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<Comment> GetCommentById(Guid CommentId)
        {
            Comment existingComment = new Comment();
            existingComment = _context.Comments.FirstOrDefault(c => c.CommentId == CommentId);
            return existingComment;
        }

        public async Task<List<Comment>> GetMovieComments(Guid movieId)
        {
            List<Comment> comments = new List<Comment>();
            comments = await _context.Comments
                .Where(c => c.MovieId == movieId)
                .ToListAsync();
            comments = comments.OrderByDescending(c => c.DatePosted).ToList();
            return comments;
        }

        public async Task UpdateComment(Comment updatedComment)
        {
            Comment existingComment = new Comment();
            existingComment = _context.Comments.FirstOrDefault(c => c.CommentId == updatedComment.CommentId);

            if (existingComment == null)
            {
                throw new ArgumentException("Comment not found");
            }
            //update the propertis of existing comment
            existingComment.UserName = updatedComment.UserName;
            existingComment.DatePosted = updatedComment.DatePosted;
            existingComment.CommentId = updatedComment.CommentId;
            existingComment.MovieId = updatedComment.MovieId;
            existingComment.Content = updatedComment.Content;

            //save changes to database
            _context.Comments.Update(existingComment);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteComment(Guid commentId)
        {
            var comment = _context.Comments.FirstOrDefault(c => c.CommentId == commentId);
            if (comment != null)
            {
                _context.Comments.Remove((Comment)comment);
                await _context.SaveChangesAsync();
            }
        }


    }
}
