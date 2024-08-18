using api.Data;
using api.DTOs.Blog;
using api.Entities;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly BlogDbContext _blogDbContext;

        public BlogRepository(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext;
        }
        public async Task<Blog> CreateBlogAsync(BlogDTO blogDTO)
        {
            var blog = new Blog
            {
                Title = blogDTO.Title,
                Content = blogDTO.Content,
                AuthorId = blogDTO.AuthorId,
                CreatedAt = blogDTO.CreatedAt,
            };

            var newBlog = await _blogDbContext.Blogs.AddAsync(blog);

            await _blogDbContext.SaveChangesAsync();

            return newBlog.Entity;
        }

        public async Task<Blog?> DeleteBlogAsync(int id)
        {
            var existingBlog = await GetBlogAsync(id);
            if (existingBlog is null) return null;
            var deletedBlog = _blogDbContext.Blogs.Remove(existingBlog);

            await _blogDbContext.SaveChangesAsync();

            return deletedBlog.Entity;
        }

        public async Task<Blog?> GetBlogAsync(int id)
        {
            var existingBlog = await _blogDbContext.Blogs.FirstOrDefaultAsync(b => b.Id == id);
            return existingBlog;
        }

        public async Task<List<Blog>> GetBlogsAsync()
        {
            return await _blogDbContext.Blogs.ToListAsync();
        }

        public async Task<Blog?> UpdateBlogAsync(int id, UpdateBlogDTO blogDTO)
        {
            var existingBlog = await GetBlogAsync(id);

            if (existingBlog is null) return null;

            existingBlog.Title = blogDTO.Title is null ? existingBlog.Title : blogDTO.Title;
            existingBlog.Content = blogDTO.Content is null ? existingBlog.Content : blogDTO.Content;
            existingBlog.AuthorId = blogDTO.AuthorId is null ? existingBlog.AuthorId : (int)blogDTO.AuthorId;

            await _blogDbContext.SaveChangesAsync();

            return existingBlog;
        }
    }
}