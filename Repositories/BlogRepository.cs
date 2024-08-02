using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Interfaces
{
    public interface IBlogRepository
    {
        public Task<Blog> CreateBlogAsync(Blog blog);
        public Task<List<Blog>> GetBlogsAsync();
    }
}

namespace api.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly BlogDbContext _context;

        public BlogRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<Blog> CreateBlogAsync(Blog blog)
        {
            try
            {
                var returnedBlog = await _context.Blogs.AddAsync(blog);
                await _context.SaveChangesAsync();
                return returnedBlog.Entity;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<Blog>> GetBlogsAsync()
        {
            try
            {
                var blogs = await _context.Blogs.ToListAsync();
                return blogs;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}