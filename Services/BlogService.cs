using api.DTOs;
using api.Interfaces;
using api.Models;

namespace api.Interfaces
{
    public interface IBlogService
    {
        Task<BlogDTO> CreateBlogAsync(Blog blog);
        Task<List<BlogDTO>> GetBlogsAsync();
    }
}

namespace api.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;

        public BlogService(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<BlogDTO> CreateBlogAsync(Blog blog)
        {
            try
            {
                var returnedBlog = await _blogRepository.CreateBlogAsync(blog);
                return new BlogDTO
                {
                    Id = returnedBlog.Id,
                    Title = returnedBlog.Title,
                    Content = returnedBlog.Content,
                    AuthorId = returnedBlog.AuthorId,
                    CreatedAt = returnedBlog.CreatedAt,
                    UpdatedAt = returnedBlog.UpdatedAt
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<BlogDTO>> GetBlogsAsync()
        {
            try
            {
                var blogs = await _blogRepository.GetBlogsAsync();
                return blogs.Select(blog => new BlogDTO
                {
                    Id = blog.Id,
                    Title = blog.Title,
                    Content = blog.Content,
                    AuthorId = blog.AuthorId,
                    CreatedAt = blog.CreatedAt,
                    UpdatedAt = blog.UpdatedAt
                }).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}