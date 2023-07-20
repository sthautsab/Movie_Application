using Movie_Application.Data;
using Movie_Application.Models;
using Movie_Application.Repository.Interface;

namespace Movie_Application.Repository.Implementation
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieContext _movieContext;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public MovieRepository(MovieContext movieContext, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _movieContext = movieContext;
            _hostingEnvironment = hostingEnvironment;

        }
        public async Task<bool> AddMovie(Movie movie)
        {
            _movieContext.Movies.Add(movie);
            int entitiesAdded = await _movieContext.SaveChangesAsync();
            bool success = entitiesAdded > 0;
            return success;
        }

        public async Task<Movie> GetMovieById(Guid id)
        {
            var movie = _movieContext.Movies.FirstOrDefault(m => m.Id == id);
            if (movie != null)
            {
                return movie;
            }
            else
            {
                return null;
            }
        }
        public List<Movie> GetMovies()
        {
            List<Movie> movies = new List<Movie>();
            movies = _movieContext.Movies.ToList();
            return movies;
        }

        public async Task<bool> UpdateMovie(Movie updatedMovie)
        {

            var movie = _movieContext.Movies.FirstOrDefault(m => m.Id == updatedMovie.Id);
            if (movie != null)
            {
                movie.Name = updatedMovie.Name;
                movie.Director = updatedMovie.Director;
                movie.Genre = updatedMovie.Genre;
                movie.Description = updatedMovie.Description;
                movie.PhotoPath = updatedMovie.PhotoPath;

                _movieContext.Movies.Update(movie);
                await _movieContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }

        }


        public bool DeleteMovie(Guid id)
        {
            //Movie? movie = new Movie();
            var movie = _movieContext.Movies.FirstOrDefault(m => m.Id == id);
            if (movie != null)
            {
                _movieContext.Movies.Remove(movie);
                _movieContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

