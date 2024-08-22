using api.DTOs.ApiResponse;
using api.DTOs.Blog;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

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
    public async Task<IActionResult> CreateBlogAsync(CreateBlogRequest request)
    {
        try
        {
            var createdBlog = await _blogService.CreateBlogAsync(request.ToDTO());
            return Created("", new SuccessResponse(201, "Create blog successfully.", createdBlog));
        }
        catch (DbUpdateException ex)
        {
            var sqlException = ex.InnerException as SqlException;

            if (sqlException != null)
            {
                switch (sqlException.Number)
                {
                    case 547: // Foreign Key violation
                        return BadRequest(new FailResponse(400, "Author does not exist."));
                    case 2601: // Unique Index/Constraint violation
                    case 2627: // Primary Key violation
                        return BadRequest(new FailResponse(400, "Duplicate key violation. The record already exists."));
                    default:
                        return StatusCode(500, new FailResponse(500, "A database error occurred. Please try again later."));
                }
            }

            return StatusCode(500, new FailResponse(500, "An unexpected error occurred. Please try again later."));
        }
        catch (Exception)
        {
            return StatusCode(500, new FailResponse(500, "An unexpected error occurred. Please try again later."));
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetBlogsAsync()
    {
        try
        {
            var blogs = await _blogService.GetBlogsAsync();

            return Ok(new SuccessResponse(200, null, blogs));
        }
        catch (Exception)
        {
            throw;
        }
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBlogAsync(int id)
    {
        try
        {
            var blog = await _blogService.GetBlogAsync(id);
            if (blog is null)
            {
                return NotFound(new FailResponse(404, "Blog not found."));
            }
            return Ok(new SuccessResponse(200, "Get blog successfully.", blog));
        }
        catch (Exception)
        {
            throw;
        }
    }
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateBlogFieldsAsync(int id, UpdateBlogRequest requestBody)
    {
        try
        {
            var updatedBlog = await _blogService.UpdateBlogAsync(id, requestBody.ToUpdateDTO());
            if (updatedBlog is null)
            {
                return NotFound(new FailResponse(404, "Blog not found."));
            }
            return Ok(new SuccessResponse(200, "Update blog successfully.", updatedBlog));
        }
        catch (Exception)
        {
            throw;
        }
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBlogAsync(int id, CreateBlogRequest requestBody)
    {
        try
        {
            var updatedBlog = await _blogService.UpdateBlogAsync(id, requestBody.ToUpdateDTO());
            if (updatedBlog is null)
            {
                return NotFound(new FailResponse(404, "Blog not found."));
            }
            return Ok(new SuccessResponse(200, "Update blog successfully.", updatedBlog));
        }
        catch (Exception)
        {
            throw;
        }
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBlogAsync(int id)
    {
        try
        {
            var deletedBlog = await _blogService.DeleteBlogAsync(id);
            if (deletedBlog == null)
            {
                return NotFound(new FailResponse(404, "Blog not found."));
            }

            return Ok(new SuccessResponse(200, "Delete blog successfully.", deletedBlog));
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new FailResponse(500, "A database error occurred. Please try again later."));
        }
        catch (Exception)
        {
            return StatusCode(500, new FailResponse(500, "An unexpected error occurred. Please try again later."));
        }
    }
}