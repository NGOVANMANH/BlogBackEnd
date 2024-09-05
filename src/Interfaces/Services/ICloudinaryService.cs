namespace api.Interfaces
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageFile(IFormFile file);
        Task<string> UploadVideoFile(IFormFile file);
        Task<bool> DeleteResource(string publicId, string resourceType);
        Task<string> GetImageDetails(string publicId);
        Task<string> GetVideoDetails(string publicId);
        Task<string> UpdateImageTransformation(string publicId, string transformation);
        Task<string> UpdateVideoTransformation(string publicId, string transformation);
    }
}
