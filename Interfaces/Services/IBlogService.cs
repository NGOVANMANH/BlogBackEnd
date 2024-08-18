using api.DTOs.Blog;

namespace api.Interfaces
{
    public interface IBlogService
    {
        Task<BlogDTO> CreateBlogAsync(BlogDTO blogDTO);
        Task<List<BlogDTO>> GetBlogsAsync();
        Task<BlogDTO?> GetBlogAsync(int id);
        Task<BlogDTO?> UpdateBlogAsync(int id, UpdateBlogDTO blogDTO);
        Task<BlogDTO?> DeleteBlogAsync(int id);
    }
}