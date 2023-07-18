using Microsoft.AspNetCore.Mvc;
using Movie_Application.Data;
using Movie_Application.Models;
using Movie_Application.Repository.Interface;

namespace Movie_Application.Repository.Implementation
{
    public class RatingRepository : IRatingRepository
    {
        private readonly MovieContext _context;
        public RatingRepository(MovieContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task AddRating(Rating rating)
        {
            Rating rate = new Rating();
            rate.UserId = rating.UserId;
            rate.MovieId = rating.MovieId;
            rate.Rate = rating.Rate;
            await _context.Ratings.AddAsync(rate);
            await _context.SaveChangesAsync();
        }
    }
}
