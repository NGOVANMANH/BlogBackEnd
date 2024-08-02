using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Blog
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(200, ErrorMessage = "Title is too long")]
        public string Title { get; set; } = "";

        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; } = "";

        [Required]
        [ForeignKey(nameof(User))]
        public int AuthorId { get; set; }

        [Required]
        public User Author { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
