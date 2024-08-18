using System.ComponentModel.DataAnnotations;

namespace api.DTOs.User;

public class UpdateUserDTO
{
    public string? Username { get; set; }
    [DataType(DataType.Date)]
    public DateTime? Birthdate { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Password { get; set; }
}