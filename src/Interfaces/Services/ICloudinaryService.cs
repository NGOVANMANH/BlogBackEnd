using CloudinaryDotNet.Actions;

namespace api.Interfaces
{
    public interface ICloudinaryService
    {
        Task<ImageUploadResult> UploadImageFile(IFormFile file);
        Task<VideoUploadResult> UploadVideoFile(IFormFile file);
        Task<bool> DeleteResource(string publicId, ResourceType resourceType);
        Task<string> GetImageDetails(string publicId);
        Task<string> GetVideoDetails(string publicId);
        // Task<string> UpdateImageTransformation(string publicId, string transformation);
        // Task<string> UpdateVideoTransformation(string publicId, string transformation);
    }
}
