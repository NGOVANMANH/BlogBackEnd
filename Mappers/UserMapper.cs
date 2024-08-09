using api.DTOs.Auth;
using api.DTOs.User;
using api.Models;

namespace api.Mappers;

public static class UserMapper
{
    public static UserDTO ToDTO(User user)
    {
        return new UserDTO
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.Username,
            Birthdate = user.Birthdate,
            FirstName = user.FirstName!,
            LastName = user.LastName!,
            IsVerified = user.IsVerified
        };
    }
    public static UserDTO ToDTO(RegistrationRequest req)
    {
        return new UserDTO
        {
            Email = req.Email,
            Username = req.Username,
            Birthdate = req.Birthdate,
            FirstName = req.FirstName!,
            LastName = req.LastName!,
        };
    }
    public static User ToModel(UserDTO userDTO)
    {
        return new User
        {
            Email = userDTO.Email,
            Username = userDTO.Username,
            Birthdate = userDTO.Birthdate,
            FirstName = userDTO.FirstName,
            LastName = userDTO.LastName
        };
    }
}