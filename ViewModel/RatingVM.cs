using System.ComponentModel.DataAnnotations;

namespace Movie_Application.ViewModel
{
    public class RatingVM
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public Guid MovieId { get; set; }
        [Range(0, 5)]
        public int Rate { get; set; }
    }
}
