using System.ComponentModel.DataAnnotations;

namespace api.DTOs;

public class UserDTO
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    [DataType(DataType.Date)]
    public DateTime? Birthdate { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}