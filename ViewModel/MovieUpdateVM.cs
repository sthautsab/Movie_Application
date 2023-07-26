using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Movie_Application.ViewModel
{
    public class MovieUpdateVM
    {
        public Guid Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Genre { get; set; }

        public string? Director { get; set; }
        public string? Description { get; set; }

        public string? PhotoPath { get; set; }

        [DataType(DataType.Upload)]
        [DisplayName("Update Photo")]
        public IFormFile? Photo { get; set; }
    }
}
