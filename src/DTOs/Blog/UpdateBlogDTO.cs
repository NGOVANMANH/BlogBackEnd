using api.DTOs.User;

namespace api.DTOs.Blog;

public class UpdateBlogDTO
{
    public int? Id { get; set; }
    public string? Title { get; set; } = "";
    public string? Content { get; set; } = "";
    public int? AuthorId { get; set; }
    public UserDTO? Author { get; set; } = null!;
}