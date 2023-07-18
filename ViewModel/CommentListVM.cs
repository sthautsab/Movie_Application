using Movie_Application.Models;

namespace Movie_Application.ViewModel
{
    public class CommentListVM
    {
        public Guid MovieId { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
