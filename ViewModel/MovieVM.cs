using System.ComponentModel.DataAnnotations;

namespace Movie_Application.ViewModel
{
    public class MovieVM
    {
        public string? Name { get; set; }

        [Required]
        public string? Genre { get; set; }

        public string? Director { get; set; }
        public string? Description { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile Photo { get; set; }
    }
}
