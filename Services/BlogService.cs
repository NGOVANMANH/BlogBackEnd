using api.DTOs.Blog;
using api.Interfaces;

namespace api.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;

        public BlogService(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<BlogDTO> CreateBlogAsync(BlogDTO blogDTO)
        {
            var newBlog = await _blogRepository.CreateBlogAsync(blogDTO);

            return Mappers.BlogMapper.ToDTO(newBlog);
        }

        public async Task<BlogDTO?> DeleteBlogAsync(int id)
        {
            var deletedBlog = await _blogRepository.DeleteBlogAsync(id);

            if (deletedBlog is null) return null;

            return Mappers.BlogMapper.ToDTO(deletedBlog);
        }

        public async Task<BlogDTO?> GetBlogAsync(int id)
        {
            var blog = await _blogRepository.GetBlogAsync(id);

            if (blog is null) return null;

            return Mappers.BlogMapper.ToDTO(blog);
        }

        public async Task<List<BlogDTO>> GetBlogsAsync()
        {
            var blogs = await _blogRepository.GetBlogsAsync();

            var blogsDTO = blogs.Select(b => Mappers.BlogMapper.ToDTO(b)).ToList();

            return blogsDTO;
        }

        public async Task<BlogDTO?> UpdateBlogAsync(int id, UpdateBlogDTO blogDTO)
        {
            var updatedBlog = await _blogRepository.UpdateBlogAsync(id, blogDTO);

            if (updatedBlog is null) return null;

            return Mappers.BlogMapper.ToDTO(updatedBlog);
        }
    }
}