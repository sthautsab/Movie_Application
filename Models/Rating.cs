using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movie_Application.Models
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }
        [Required]
        [ForeignKey("Movie")]
        public Guid MovieId { get; set; }
        [Range(0, 5)]
        public int? Rate { get; set; }

        public virtual IdentityUser User { get; set; }
        public virtual Movie Movie { get; set; }
    }
}
