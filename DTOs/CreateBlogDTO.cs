using System.ComponentModel.DataAnnotations;

namespace api.DTOs;

public class CreateBlogDTO
{
    [Required(ErrorMessage = "Title is required")]
    [MaxLength(200, ErrorMessage = "Title is too long")]
    public string Title { get; set; } = "";

    [Required(ErrorMessage = "Content is required")]
    public string Content { get; set; } = "";

    [Required(ErrorMessage = "AuthorId is required")]
    public int AuthorId { get; set; }
}