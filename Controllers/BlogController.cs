using api.DTOs;
using api.Interfaces;
using api.Models;
using api.Utils;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]s")]
public class BlogController : ControllerBase
{
    private readonly IBlogService _blogService;

    public BlogController(IBlogService blogService)
    {
        _blogService = blogService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBlogAsync(CreateBlogDTO blog)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(
                new ApiResponseDTO(
                    false,
                    "Invalid data",
                    new
                    {
                        errors = ModelStateUtil.FormatModelStateErrors(ModelState)
                    }
                )
            );
        }

        try
        {
            var newBlog = new Blog
            {
                Title = blog.Title,
                Content = blog.Content,
                AuthorId = blog.AuthorId
            };

            var createdBlog = await _blogService.CreateBlogAsync(newBlog);
            return Created(
                "blog",
                new ApiResponseDTO(
                    true,
                    "Blog created successfully",
                    createdBlog
                )
            );
        }
        catch (Exception e)
        {
            return StatusCode(
                500,
                new ApiResponseDTO(
                    false,
                    e.Message
                )
            );
        }
    }
    [HttpGet]
    public async Task<IActionResult> GetBlogsAsync()
    {
        try
        {
            var blogs = await _blogService.GetBlogsAsync();
            return Ok(
                new ApiResponseDTO(
                    true,
                    "Blogs fetched successfully",
                    blogs
                )
            );
        }
        catch (Exception e)
        {
            return StatusCode(
                500,
                new ApiResponseDTO(
                    false,
                    e.Message
                )
            );
        }
    }
}