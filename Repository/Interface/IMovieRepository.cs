using Movie_Application.Models;
using Movie_Application.ViewModel;

namespace Movie_Application.Repository.Interface
{
    public interface IMovieRepository
    {
        List<Movie> GetMovies();
        Movie GetMovieById(Guid id);
        Task<bool> AddMovie(MovieViewModel model);
        Task<bool> UpdateMovie(UpdateViewModel updatedMovie);



        bool DeleteMovie(Guid id);

    }
}
