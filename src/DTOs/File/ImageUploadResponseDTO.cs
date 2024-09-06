namespace api.DTOs.File;

public class ImageUploadResponseDTO
{
    public string? Url { get; set; }            // The URL to access the image
    public string? SecureUrl { get; set; }       // Secure (HTTPS) URL
    public string? PublicId { get; set; }        // The public ID of the uploaded image
    public int Width { get; set; }              // Image width in pixels
    public int Height { get; set; }             // Image height in pixels
    public string? Format { get; set; }          // Image format (e.g., jpg, png)
    public long Bytes { get; set; }             // File size in bytes
    public DateTime CreatedAt { get; set; }     // The time when the image was uploaded
}
