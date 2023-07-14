using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie_Application.Models;
using Movie_Application.Repository.Interface;
using Movie_Application.ViewModel;

namespace Movie_Application.Controllers
{
    [Authorize]
    public class MovieController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        public MovieController(IMovieRepository movieRepository, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _movieRepository = movieRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult AddMovie()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddMovie(MovieViewModel movieVM)
        {
            string uniqueFileName = null;
            if (movieVM.Photo != null)
            {
                uniqueFileName = await UploadFile(movieVM.Photo);
            }
            Movie movie = new Movie();
            movie.Name = movieVM.Name;
            movie.Director = movieVM.Director;
            movie.Description = movieVM.Description;
            movie.PhotoPath = uniqueFileName;
            movie.Genre = movieVM.Genre;

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
            try
            {
                string uniqueFileName = null;
                //string filePath = updatedMovie.PhotoPath;
                if (updatedMovie.Photo != null)
                {
                    uniqueFileName = await UploadFile(updatedMovie.Photo);
                }
                Movie movie = new Movie();
                movie.Id = updatedMovie.Id;
                movie.Name = updatedMovie.Name;
                movie.Director = updatedMovie.Director;
                movie.Description = updatedMovie.Description;
                movie.Genre = updatedMovie.Genre;

                if (uniqueFileName != null)
                {
                    movie.PhotoPath = uniqueFileName;
                }
                else
                {
                    movie.PhotoPath = updatedMovie.PhotoPath;
                }

                bool result = await _movieRepository.UpdateMovie(movie);
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
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while updating the movie: " + ex.Message;
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

        public async Task<string> UploadFile(IFormFile file)
        {
            string uniqueFileName = null;
            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");

            uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

            string filePath = Path.Combine(uploadFolder, uniqueFileName);

            await file.CopyToAsync(new FileStream(filePath, FileMode.Create));
            return uniqueFileName;
        }

    }
}
