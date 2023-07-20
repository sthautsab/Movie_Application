using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie_Application.Models;
using Movie_Application.Repository.Interface;
using Movie_Application.ViewModel;
using System.Security.Claims;

namespace Movie_Application.Controllers
{

    public class MovieController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly IRatingRepository _ratingRepository;

        public MovieController(IMovieRepository movieRepository, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IRatingRepository ratingRepository)
        {
            _movieRepository = movieRepository;
            _hostingEnvironment = hostingEnvironment;
            _ratingRepository = ratingRepository;
        }

        public IActionResult AddMovie()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
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
        public IActionResult GetMovies(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 3;



            List<Movie> movies = new List<Movie>();
            movies = _movieRepository.GetMovies();

            int totalMovies = movies.Count;
            int totalPages = (int)Math.Ceiling(totalMovies / (double)pageSize);

            //starting index of each page
            int startIndex = (pageNumber - 1) * pageSize;

            //skip skips first specified number of data and take takes the specified number of data
            List<Movie> pagedMovies = movies.Skip(startIndex).Take(pageSize).ToList();

            PagedMovieVM pagedMovieVM = new PagedMovieVM
            {
                Movies = pagedMovies,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalMovies = totalMovies,
                TotalPages = totalPages
            };

            return View(pagedMovieVM);
        }

        public async Task<IActionResult> GetMovieById(Guid id)
        {
            MovieCommentVM details = new MovieCommentVM();
            RatingVM ratingVM = new RatingVM();
            Movie movie = new Movie();
            movie = await _movieRepository.GetMovieById(id);



            var UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(UserId))
            {
                var rate = (int)await _ratingRepository.GetRatingByUserIdAndMovieId(UserId, id);
                if (rate != 0)
                {
                    ratingVM.Rate = rate;
                }
                else
                {
                    ratingVM.Rate = 0;
                }
                ratingVM.MovieId = id;
                ratingVM.UserId = UserId;
            }
            if (movie != null)
            {
                details.Movie = movie;
                details.RatingVM = ratingVM;
                return View(details);
            }
            else
            {
                return RedirectToAction("GetMovies");
            }

        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var movie = await _movieRepository.GetMovieById(id);
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
