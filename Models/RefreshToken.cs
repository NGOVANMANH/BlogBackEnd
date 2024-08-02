using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

public class RefreshToken
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Token { get; set; } = "";

    [Required]
    public DateTime Expires { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    [ForeignKey(nameof(User))]
    public int CreatedBy { get; set; }

    public DateTime? Revoked { get; set; }

    [MaxLength(100)]
    public string? ReplacedByToken { get; set; }

    public bool IsExpired => DateTime.UtcNow >= Expires;

    public bool IsActive => Revoked == null && !IsExpired;
}