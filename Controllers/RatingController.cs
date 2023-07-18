using Microsoft.AspNetCore.Mvc;
using Movie_Application.ViewModel;

namespace Movie_Application.Controllers
{
    public class RatingController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> AddRating([Bind("MovieId, Rate")] RatingVM rating)
        {
            return PartialView("~/Views/Rating/_AddRating.cshtml");
        }
    }
}
