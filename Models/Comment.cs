using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movie_Application.Models
{
    public class Comment
    {
        [Key]
        public Guid CommentId { get; set; }

        public Guid MovieId { get; set; }
        [ForeignKey("MovieId")]

        public Movie? Movie { get; set; }
        public string? UserName { get; set; }
        public string Content { get; set; }
        public DateTime DatePosted { get; set; }
    }
}
