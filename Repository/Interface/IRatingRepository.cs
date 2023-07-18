using Movie_Application.Models;

namespace Movie_Application.Repository.Interface
{
    public interface IRatingRepository
    {
        Task AddRating(Rating rating);
    }
}
