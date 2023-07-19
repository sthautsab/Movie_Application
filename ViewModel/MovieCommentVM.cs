using Movie_Application.Models;

namespace Movie_Application.ViewModel
{
    public class MovieCommentVM
    {
        public Movie Movie { get; set; }
        public CommentVM? CommentVM { get; set; }
        public RatingVM? RatingVM { get; set; }

    }
}
