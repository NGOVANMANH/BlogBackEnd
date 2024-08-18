using System.ComponentModel.DataAnnotations;
using api.DTOs.User;

namespace api.DTOs.Blog;

public class UpdateBlogRequest
{
    public string? Title { get; set; } = null;
    public string? Content { get; set; } = null;
    public int? AuthorId { get; set; } = null;
}