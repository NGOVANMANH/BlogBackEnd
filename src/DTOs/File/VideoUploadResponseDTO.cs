namespace api.DTOs.File;
public class VideoUploadResponseDTO
{
    public string? Url { get; set; }
    public string? SecureUrl { get; set; }
    public string? PublicId { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public double Duration { get; set; }
    public double FrameRate { get; set; }
    public long BitRate { get; set; }
    public string? Format { get; set; }
    public string? Codec { get; set; }
    public string? Profile { get; set; }
    public string? PlaybackUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public long Bytes { get; set; }
    public string? OriginalFilename { get; set; }
}
