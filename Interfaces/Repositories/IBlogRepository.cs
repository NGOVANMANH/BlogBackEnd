using api.DTOs.Blog;
using api.Entities;

namespace api.Interfaces
{
    public interface IBlogRepository
    {
        Task<Blog> CreateBlogAsync(BlogDTO blogDTO);
        Task<List<Blog>> GetBlogsAsync();
        Task<Blog?> GetBlogAsync(int id);
        Task<Blog?> UpdateBlogAsync(int id, UpdateBlogDTO blogDTO);
        Task<Blog?> DeleteBlogAsync(int id);
    }
}