using Movie_Application.Models;

namespace Movie_Application.ViewModel
{
    public class MovieDetailsVM
    {
        public Movie Movie { get; set; }
        public CommentVM? CommentVM { get; set; }
        public RatingVM? RatingVM { get; set; }
        public List<CommentVM> comments { get; set; }

    }
}
