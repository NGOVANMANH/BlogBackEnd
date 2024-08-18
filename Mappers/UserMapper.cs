using api.DTOs.Auth;
using api.DTOs.User;
using api.Entities;

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
            Birthday = user.Birthday,
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
            Birthday = req.Birthdate,
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
            Birthday = userDTO.Birthday,
            FirstName = userDTO.FirstName,
            LastName = userDTO.LastName
        };
    }
}