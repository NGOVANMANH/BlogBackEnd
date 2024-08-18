using System.ComponentModel.DataAnnotations;

namespace api.DTOs.User;

public class UserDTO
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    [DataType(DataType.Date)]
    public DateTime? Birthday { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool IsVerified { get; set; }
}