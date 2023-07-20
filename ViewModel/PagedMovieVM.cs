using Movie_Application.Models;

namespace Movie_Application.ViewModel
{
    public class PagedMovieVM
    {
        public List<Movie> Movies { get; set; }
        public Movie Movie { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalMovies { get; set; }
        public int TotalPages { get; set; }
    }
}
