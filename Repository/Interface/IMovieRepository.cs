using Movie_Application.Models;

namespace Movie_Application.Repository.Interface
{
    public interface IMovieRepository
    {
        List<Movie> GetMovies();
        Task<Movie> GetMovieById(Guid id);
        Task<bool> AddMovie(Movie model);
        Task<bool> UpdateMovie(Movie updatedMovie);
        bool DeleteMovie(Guid id);

    }
}
