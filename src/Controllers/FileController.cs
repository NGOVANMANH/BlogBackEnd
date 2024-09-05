using api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly ICloudinaryService _cloudinaryService;

        public FileController(ICloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File not provided or empty.");
            }

            try
            {
                var url = await _cloudinaryService.UploadImageFile(file);
                return Ok(new { Url = url });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("upload-video")]
        public async Task<IActionResult> UploadVideo(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File not provided or empty.");
            }

            try
            {
                var url = await _cloudinaryService.UploadVideoFile(file);
                return Ok(new { Url = url });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("delete-resource")]
        public async Task<IActionResult> DeleteResource(string publicId, string resourceType)
        {
            try
            {
                var result = await _cloudinaryService.DeleteResource(publicId, resourceType);
                if (result)
                {
                    return Ok("Resource deleted successfully.");
                }
                return StatusCode(500, "Failed to delete resource.");
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("image-details")]
        public async Task<IActionResult> GetImageDetails(string publicId)
        {
            try
            {
                var details = await _cloudinaryService.GetImageDetails(publicId);
                return Ok(details);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("video-details")]
        public async Task<IActionResult> GetVideoDetails(string publicId)
        {
            try
            {
                var details = await _cloudinaryService.GetVideoDetails(publicId);
                return Ok(details);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("update-image-transformation")]
        public async Task<IActionResult> UpdateImageTransformation(string publicId, string transformation)
        {
            try
            {
                var result = await _cloudinaryService.UpdateImageTransformation(publicId, transformation);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("update-video-transformation")]
        public async Task<IActionResult> UpdateVideoTransformation(string publicId, string transformation)
        {
            try
            {
                var result = await _cloudinaryService.UpdateVideoTransformation(publicId, transformation);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
