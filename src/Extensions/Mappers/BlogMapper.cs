using api.DTOs.Blog;
using api.Entities;

namespace api.Mappers;

public static class BlogMapper
{
    public static BlogDTO ToDTO(this Blog blog)
    {
        return new BlogDTO
        {
            Id = blog.Id,
            Author = Mappers.UserMapper.ToDTO(blog.Author),
            AuthorId = blog.AuthorId,
            Content = blog.Content,
            CreatedAt = blog.CreatedAt,
            Title = blog.Title,
        };
    }
    public static BlogDTO ToDTO(this CreateBlogRequest blog)
    {
        return new BlogDTO
        {
            AuthorId = blog.AuthorId,
            Content = blog.Content,
            Title = blog.Title,
        };
    }
    public static UpdateBlogDTO ToUpdateDTO(this UpdateBlogRequest blog)
    {
        return new UpdateBlogDTO
        {
            AuthorId = blog.AuthorId,
            Content = blog.Content,
            Title = blog.Title,
        };
    }
    public static UpdateBlogDTO ToUpdateDTO(this CreateBlogRequest blog)
    {
        return new UpdateBlogDTO
        {
            AuthorId = blog.AuthorId,
            Content = blog.Content,
            Title = blog.Title,
        };
    }
}