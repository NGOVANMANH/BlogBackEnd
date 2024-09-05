using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]s")]
public class CommentController : ControllerBase
{
    [HttpPost]
    public IActionResult CreateCommentAsync()
    {
        return Ok("From C comment");
    }
    [HttpGet("{id}")]
    public IActionResult GetCommentAsync(string id)
    {
        return Ok("From R comment");
    }
    [HttpGet]
    public IActionResult GetCommentsAsync()
    {
        return Ok("From R comment");
    }
    [HttpPatch("{id}")]
    public IActionResult UpdateCommentAsync(string id, Object requestBody)
    {
        return Ok("From C comment");
    }
    [HttpDelete("{id}")]
    public IActionResult DeleteCommentAsync(string id)
    {
        return Ok("From C comment");
    }
}