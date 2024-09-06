using api.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace api.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<ImageUploadResult> UploadImageFile(IFormFile file)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Folder = "images" // Optional: specify a folder for images
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult;
            }

            throw new Exception("Error uploading image to Cloudinary.");
        }

        public async Task<VideoUploadResult> UploadVideoFile(IFormFile file)
        {
            var uploadParams = new VideoUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Folder = "videos" // Optional: specify a folder for videos
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult;
            }

            throw new Exception("Error uploading video to Cloudinary.");
        }

        public async Task<bool> DeleteResource(string publicId, ResourceType resourceType)
        {
            var deletionParams = new DeletionParams(publicId)
            {
                ResourceType = resourceType
            };

            var deletionResult = await _cloudinary.DestroyAsync(deletionParams);
            return deletionResult.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<string> GetImageDetails(string publicId)
        {
            var getResourceParams = new GetResourceParams(publicId)
            {
                ResourceType = ResourceType.Image
            };

            var resourceResult = await _cloudinary.GetResourceAsync(getResourceParams);
            return resourceResult.JsonObj.ToString();
        }

        public async Task<string> GetVideoDetails(string publicId)
        {
            var getResourceParams = new GetResourceParams(publicId)
            {
                ResourceType = ResourceType.Video
            };

            var resourceResult = await _cloudinary.GetResourceAsync(getResourceParams);
            return resourceResult.JsonObj.ToString();
        }

        // public async Task<string> UpdateImageTransformation(string publicId, string transformation)
        // {
        //     var updateParams = new UpdateParams(publicId)
        //     {
        //         ResourceType = ResourceType.Image
        //     };

        //     var updateResult = await _cloudinary.UpdateResourceAsync(updateParams);
        //     return updateResult.JsonObj.ToString();
        // }

        // public async Task<string> UpdateVideoTransformation(string publicId, string transformation)
        // {
        //     var updateParams = new UpdateParams(publicId)
        //     {
        //         ResourceType = ResourceType.Video
        //     };

        //     var updateResult = await _cloudinary.UpdateResourceAsync(updateParams);
        //     return updateResult.JsonObj.ToString();
        // }
    }
}
