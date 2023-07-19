using Microsoft.AspNetCore.Mvc;
using Movie_Application.Models;
using Movie_Application.Repository.Interface;
using Movie_Application.ViewModel;
using System.Security.Claims;

namespace Movie_Application.Controllers
{
    public class RatingController : Controller
    {
        private readonly IRatingRepository _ratingRepository;
        public RatingController(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }
        [HttpPost]
        public async Task<IActionResult> AddRating([Bind("MovieId, Rate")] RatingVM ratingVM)
        {
            Rating rating = new Rating();
            rating.MovieId = ratingVM.MovieId;
            rating.Rate = ratingVM.Rate;
            rating.UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            //check if that movie has been rated by that user
            int? rate = await _ratingRepository.GetRatingByUserIdAndMovieId(rating.UserId, rating.MovieId);
            //rate is 0 if not rated 
            if (rate == 0)
            {
                await _ratingRepository.AddRating(rating);
            }
            else
            {
                await _ratingRepository.UpdateUserRating(rating);
            }

            return RedirectToAction("GetMovieById", "Movie", new { id = ratingVM.MovieId });
            //return PartialView("~/Views/Rating/_AddRating.cshtml", ratingVM);
        }
    }
}
