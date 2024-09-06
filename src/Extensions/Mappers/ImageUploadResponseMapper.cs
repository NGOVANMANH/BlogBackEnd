using api.DTOs.File;
using CloudinaryDotNet.Actions;

namespace api.Mappers
{
    public static class ImageUploadResponseMapper
    {
        public static ImageUploadResponseDTO? ToDTO(this ImageUploadResult result)
        {
            if (result == null) return null;

            return new ImageUploadResponseDTO
            {
                Url = result.Url.ToString() ?? string.Empty,
                SecureUrl = result.SecureUrl.ToString() ?? string.Empty,
                PublicId = result.PublicId ?? string.Empty,
                Width = result.Width,
                Height = result.Height,
                Format = result.Format ?? string.Empty,
                Bytes = result.Bytes,
                CreatedAt = result.CreatedAt
            };
        }
    }
}
