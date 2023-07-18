using System.ComponentModel.DataAnnotations;

namespace Movie_Application.Models
{
    public class Movie
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Genre { get; set; }

        public string? Director { get; set; }
        public string? Description { get; set; }

        public string? PhotoPath { get; set; }

        public ICollection<Comment> Comments { get; set; }

    }
}
