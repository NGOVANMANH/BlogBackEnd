using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace api.Entities;

public class User
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "Username is required")]
    [MaxLength(100, ErrorMessage = "Username is too long")]
    public string Username { get; set; } = "";
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is not valid")]
    [MaxLength(100, ErrorMessage = "Email is too long")]
    public string Email { get; set; } = "";
    [Required(ErrorMessage = "Password is required")]
    [MaxLength(250, ErrorMessage = "Password is too long")]
    public string Password { get; set; } = "";
    [DataType(DataType.Date)]
    public DateTime? Birthday { get; set; }
    [MaxLength(100, ErrorMessage = "First name is too long")]
    public string? FirstName { get; set; }
    [MaxLength(100, ErrorMessage = "Last name is too long")]
    public string? LastName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; } = new();
    [DefaultValue(false)]
    public bool IsVerified { get; set; }
}