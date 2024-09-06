using api.DTOs.ApiResponse;
using api.DTOs.File;
using api.Interfaces;
using api.Mappers;
using CloudinaryDotNet.Actions;
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
                return BadRequest(new FailResponse().GetInvalidResponse("File not provided or empty."));
            }

            // Check if the file is an image by checking its MIME type
            var supportedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/bmp" };
            if (!supportedTypes.Contains(file.ContentType.ToLower()))
            {
                return BadRequest(new FailResponse().GetInvalidResponse("Invalid file format. Only image files are allowed."));
            }

            try
            {
                var result = await _cloudinaryService.UploadImageFile(file);
                return Ok(new SuccessResponse(200, null, result.ToDTO()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new FailResponse(500, ex.Message));
            }
        }

        [HttpPost("upload-video")]
        public async Task<IActionResult> UploadVideo(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new FailResponse().GetInvalidResponse("File not provided or empty."));
            }

            // Check if the file is a video by checking its MIME type
            var supportedTypes = new[] { "video/mp4", "video/avi", "video/mpeg", "video/quicktime" };
            if (!supportedTypes.Contains(file.ContentType.ToLower()))
            {
                return BadRequest(new FailResponse().GetInvalidResponse("Invalid file format. Only video files are allowed."));
            }

            try
            {
                var result = await _cloudinaryService.UploadVideoFile(file);
                return Ok(new SuccessResponse(200, null, result.ToDTO()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("delete-resource/{publicId}")]
        public async Task<IActionResult> DeleteResource(string publicId, string resourceType)
        {
            try
            {
                // Parse the resourceType string to the enum value
                if (!Enum.TryParse(resourceType, true, out ResourceType parsedResourceType))
                {
                    return BadRequest(new FailResponse().GetInvalidResponse("Invalid resource type provided."));
                }

                var result = await _cloudinaryService.DeleteResource(publicId, parsedResourceType);

                if (result)
                {
                    return Ok(new SuccessResponse(200, "Resource deleted successfully."));
                }
                return StatusCode(500, new FailResponse().GetInternalServerError("Failed to delete resource."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new FailResponse().GetInternalServerError(ex.Message));
            }
        }

        [HttpGet("image-details")]
        public async Task<IActionResult> GetImageDetails(string publicId)
        {
            try
            {
                var details = await _cloudinaryService.GetImageDetails(publicId);
                return Ok(new SuccessResponse(200, null, details));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new FailResponse().GetInternalServerError(ex.Message));
            }
        }

        [HttpGet("video-details")]
        public async Task<IActionResult> GetVideoDetails(string publicId)
        {
            try
            {
                var details = await _cloudinaryService.GetVideoDetails(publicId);
                return Ok(new SuccessResponse(200, null, details));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new FailResponse().GetInternalServerError(ex.Message));
            }
        }

        // Upload multiple images with error handling and parallel execution
        [HttpPost("upload-multiple-images")]
        public async Task<IActionResult> UploadMultipleImages(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest(new FailResponse().GetInvalidResponse("No files provided or files are empty."));
            }

            var supportedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/bmp" };
            var uploadTasks = new List<Task<ImageUploadResponseDTO?>>();
            var failedUploads = new List<string>();

            foreach (var file in files)
            {
                if (!supportedTypes.Contains(file.ContentType.ToLower()))
                {
                    failedUploads.Add($"File '{file.FileName}' has an invalid format.");
                    continue;
                }

                uploadTasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        var result = await _cloudinaryService.UploadImageFile(file);
                        return result.ToDTO(); // Success
                    }
                    catch (Exception ex)
                    {
                        failedUploads.Add($"File '{file.FileName}' failed to upload: {ex.Message}");
                        return null; // Failure
                    }
                }));
            }

            var uploadResults = await Task.WhenAll(uploadTasks);
            var successfulUploads = uploadResults.Where(result => result != null).ToList();

            if (successfulUploads.Count == 0)
            {
                return BadRequest(new FailResponse().GetInvalidResponse("No files were successfully uploaded.", failedUploads));
            }

            return Ok(new SuccessResponse(200, "Some files uploaded successfully", new
            {
                Success = successfulUploads,
                Errors = failedUploads
            }));
        }

        // Upload multiple videos with error handling and parallel execution
        [HttpPost("upload-multiple-videos")]
        public async Task<IActionResult> UploadMultipleVideos(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest(new FailResponse().GetInvalidResponse("No files provided or files are empty."));
            }

            var supportedTypes = new[] { "video/mp4", "video/avi", "video/mpeg", "video/quicktime" };
            var uploadTasks = new List<Task<VideoUploadResponseDTO?>>();
            var failedUploads = new List<string>();

            foreach (var file in files)
            {
                if (!supportedTypes.Contains(file.ContentType.ToLower()))
                {
                    failedUploads.Add($"File '{file.FileName}' has an invalid format.");
                    continue;
                }

                uploadTasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        var result = await _cloudinaryService.UploadVideoFile(file);
                        return result.ToDTO(); // Success
                    }
                    catch (Exception ex)
                    {
                        failedUploads.Add($"File '{file.FileName}' failed to upload: {ex.Message}");
                        return null; // Failure
                    }
                }));
            }

            var uploadResults = await Task.WhenAll(uploadTasks);
            var successfulUploads = uploadResults.Where(result => result != null).ToList();

            if (successfulUploads.Count == 0)
            {
                return BadRequest(new FailResponse().GetInvalidResponse("No files were successfully uploaded.", failedUploads));
            }

            return Ok(new SuccessResponse(200, "Some files uploaded successfully", new
            {
                Success = successfulUploads,
                Errors = failedUploads
            }));
        }


        // [HttpPut("update-image-transformation/{publicId}")]
        // public async Task<IActionResult> UpdateImageTransformation(string publicId, string transformation)
        // {
        //     try
        //     {
        //         var result = await _cloudinaryService.UpdateImageTransformation(publicId, transformation);
        //         return Ok(result);
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, ex.Message);
        //     }
        // }

        // [HttpPut("update-video-transformation")]
        // public async Task<IActionResult> UpdateVideoTransformation(string publicId, string transformation)
        // {
        //     try
        //     {
        //         var result = await _cloudinaryService.UpdateVideoTransformation(publicId, transformation);
        //         return Ok(result);
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, ex.Message);
        //     }
        // }
    }
}
