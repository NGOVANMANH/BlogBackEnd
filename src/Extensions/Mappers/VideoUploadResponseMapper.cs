using api.DTOs.File;
using CloudinaryDotNet.Actions;

namespace api.Mappers
{
    public static class VideoUploadResponseMapper
    {
        public static VideoUploadResponseDTO? ToDTO(this VideoUploadResult result)
        {
            if (result == null) return null;

            return new VideoUploadResponseDTO
            {
                Url = result.Url.ToString() ?? string.Empty,
                SecureUrl = result.SecureUrl.ToString() ?? string.Empty,
                PublicId = result.PublicId ?? string.Empty,
                Width = result.Width,
                Height = result.Height,
                Duration = result.Duration,
                FrameRate = result.FrameRate,
                BitRate = result.BitRate,
                Format = result.Format ?? string.Empty,
                Codec = result.Video?.Codec ?? string.Empty,
                Profile = result.Video?.Profile ?? string.Empty,
                PlaybackUrl = result.PlaybackUrl ?? string.Empty,
                CreatedAt = result.CreatedAt,
                Bytes = result.Bytes,
                OriginalFilename = result.OriginalFilename ?? string.Empty
            };
        }
    }
}
