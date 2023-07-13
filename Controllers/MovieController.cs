using Microsoft.AspNetCore.Mvc;
using Movie_Application.Models;
using Movie_Application.Repository.Interface;
using Movie_Application.ViewModel;

namespace Movie_Application.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        public MovieController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public IActionResult AddMovie()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddMovie(MovieViewModel movie)
        {

            bool result = await _movieRepository.AddMovie(movie);
            if (result == true)
            {
                TempData["SuccessMessage"] = "Movie created successfully!";
                return RedirectToAction("GetMovies");
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while creating the movie: ";
                return View();
            }

        }

        [HttpGet]
        public IActionResult GetMovies()
        {
            List<Movie> movies = new List<Movie>();
            movies = _movieRepository.GetMovies();
            return View(movies);
        }

        public IActionResult GetMovieById(Guid id)
        {
            Movie movie = new Movie();
            movie = _movieRepository.GetMovieById(id);
            if (movie != null)
            {
                return View(movie);
            }
            else
            {
                return RedirectToAction("GetMovies");
            }
        }

        public IActionResult Edit(Guid id)
        {
            var movie = _movieRepository.GetMovieById(id);
            UpdateViewModel updateMovie = new UpdateViewModel();
            updateMovie.Id = movie.Id;
            updateMovie.Name = movie.Name;
            updateMovie.Director = movie.Director;
            updateMovie.Description = movie.Description;
            updateMovie.Genre = movie.Genre;
            updateMovie.PhotoPath = movie.PhotoPath;

            return View(updateMovie);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateViewModel updatedMovie)
        {

            bool result = await _movieRepository.UpdateMovie(updatedMovie);
            if (result == true)
            {
                TempData["SuccessMessage"] = "Movie updated successfully!";
                return RedirectToAction("GetMovies");
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while updating the movie: ";
                return View();
            }
        }

        public IActionResult Delete(Guid id)
        {
            bool result = _movieRepository.DeleteMovie(id);
            if (result == true)
            {
                TempData["SuccessMessage"] = "Movie deleted successfully!";
                return RedirectToAction("GetMovies");
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the movie: ";
                ViewBag.Error = "Error while deleting";
                return View("error");
            }
        }



    }
}
