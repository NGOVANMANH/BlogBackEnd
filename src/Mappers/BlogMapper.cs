using api.DTOs.Blog;
using api.Entities;

namespace api.Mappers;

public static class BlogMapper
{
    public static BlogDTO ToDTO(Blog blog)
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
    public static BlogDTO ToDTO(CreateBlogRequest blog)
    {
        return new BlogDTO
        {
            AuthorId = blog.AuthorId,
            Content = blog.Content,
            Title = blog.Title,
        };
    }
    public static UpdateBlogDTO ToUpdateDTO(UpdateBlogRequest blog)
    {
        return new UpdateBlogDTO
        {
            AuthorId = blog.AuthorId,
            Content = blog.Content,
            Title = blog.Title,
        };
    }
    public static UpdateBlogDTO ToUpdateDTO(CreateBlogRequest blog)
    {
        return new UpdateBlogDTO
        {
            AuthorId = blog.AuthorId,
            Content = blog.Content,
            Title = blog.Title,
        };
    }
}