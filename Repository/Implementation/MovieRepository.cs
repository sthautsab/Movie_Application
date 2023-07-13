using Movie_Application.Data;
using Movie_Application.Models;
using Movie_Application.Repository.Interface;
using Movie_Application.ViewModel;

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
        public async Task<bool> AddMovie(MovieViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");

                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;

                string filePath = Path.Combine(uploadFolder, uniqueFileName);

                await model.Photo.CopyToAsync(new FileStream(filePath, FileMode.Create));
            }

            Movie movie = new Movie();
            movie.Name = model.Name;
            movie.Director = model.Director;
            movie.Description = model.Description;
            movie.PhotoPath = uniqueFileName;
            movie.Genre = model.Genre;
            _movieContext.Movies.Add(movie);
            int entitiesAdded = await _movieContext.SaveChangesAsync();
            bool success = entitiesAdded > 0;
            return success;
        }

        public Movie GetMovieById(Guid id)
        {
            //Movie? movie = new Movie();
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

        public async Task<bool> UpdateMovie(UpdateViewModel updatedMovie)
        {
            string uniqueFileName = null;
            //string filePath = updatedMovie.PhotoPath;
            if (updatedMovie.Photo != null)
            {

                string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");

                if (updatedMovie.PhotoPath != null)
                {
                    string existingFilePath = Path.Combine(uploadFolder, updatedMovie.PhotoPath);
                    //bool fileExists = File.Exists(existingFilePath);
                    //if (fileExists)
                    //{
                    //    File.Delete(existingFilePath);
                    //}
                }

                uniqueFileName = Guid.NewGuid().ToString() + "_" + updatedMovie.Photo.FileName;

                string filePath = Path.Combine(uploadFolder, uniqueFileName);

                await updatedMovie.Photo.CopyToAsync(new FileStream(filePath, FileMode.Create));
            }
            var movie = _movieContext.Movies.FirstOrDefault(m => m.Id == updatedMovie.Id);
            if (movie != null)
            {
                movie.Name = updatedMovie.Name;
                movie.Director = updatedMovie.Director;
                movie.Genre = updatedMovie.Genre;
                movie.Description = updatedMovie.Description;
                if (uniqueFileName != null)
                {
                    movie.PhotoPath = uniqueFileName;
                }
                else
                {
                    movie.PhotoPath = updatedMovie.PhotoPath;
                }

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

