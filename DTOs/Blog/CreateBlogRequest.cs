using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Blog;

public class CreateBlogRequest
{
    [Required]
    [MaxLength(255)]
    public string Title { get; set; } = "";
    [Required]
    public string Content { get; set; } = "";
    [Required]
    public int AuthorId { get; set; }
}