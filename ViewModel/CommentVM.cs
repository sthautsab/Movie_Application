using System.ComponentModel.DataAnnotations;

namespace Movie_Application.ViewModel
{
    public class CommentVM
    {
        [Key]
        public Guid CommentId { get; set; }
        public Guid MovieId { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
