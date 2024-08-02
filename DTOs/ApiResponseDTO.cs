using Microsoft.EntityFrameworkCore.Storage;

namespace api.DTOs;

public class ApiResponseDTO
{
    public string Message { get; set; }
    public bool Success { get; set; }
    public object? Data { get; set; }
    public ApiResponseDTO(bool success = true, string message = "", object? data = null)
    {
        Message = message;
        Data = data;
        Success = success;
    }
}