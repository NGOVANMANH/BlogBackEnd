using api.Data;
using api.DTOs;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;
using api.Utils;
using api.Exceptions;
using api.DTOs.Auth;
using api.DTOs.User;
using api.Mappers;

namespace api.Interfaces
{
    public interface IUserRepository
    {
        Task<User> LoginUserAsync(LoginRequest loginRequest);
        Task<User> RegisterUserAsync(RegistrationRequest registrationRequest);
        Task<User> VerifyUserAsync(string email);
        Task<User> UpdateUserAsync(int id, UpdateUserDTO updateUserDTO);
        Task<User> CreateUserAsync(UserDTO user);
        Task<User> FindUserByIdAsync(int id);
        Task<User> FindUserByEmailAsync(string email);
        Task<User> FindUserByUsernameAsync(string username);
    }
}

namespace api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BlogDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(BlogDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<User> CreateUserAsync(UserDTO userDTO)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(
                u => u.Id == userDTO.Id ||
                u.Email == userDTO.Email ||
                u.Username == userDTO.Username);

            if (existingUser is not null)
            {
                throw new AlreadyExistException("User already exist");
            }

            try
            {
                var user = UserMapper.ToModel(userDTO);

                var newUser = await _context.Users.AddAsync(user);

                await _context.SaveChangesAsync();

                return newUser.Entity;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task<User> FindUserByEmailAsync(string email)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (existingUser is null)
            {
                throw new NotFoundException();
            }

            return existingUser;
        }

        public async Task<User> FindUserByIdAsync(int id)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (existingUser is null)
            {
                throw new NotFoundException();
            }

            return existingUser;
        }

        public async Task<User> FindUserByUsernameAsync(string username)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (existingUser is null)
            {
                throw new NotFoundException();
            }

            return existingUser;
        }

        public async Task<User> LoginUserAsync(LoginRequest loginRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);
            if (user == null || BcryptUtil.VerifyPassword(loginRequest.Password!, user.Password) == false)
            {
                throw new UserNotExistException();
            }
            else if (user.IsVerified == false)
            {
                throw new UserNotVerifiedException();
            }
            return user;
        }

        public async Task<User> RegisterUserAsync(RegistrationRequest registrationRequest)
        {
            var hashedPassword = BcryptUtil.HashPassword(registrationRequest.Password!);
            var user = new User
            {
                Email = registrationRequest.Email!,
                Username = registrationRequest.Username!,
                Password = hashedPassword,
                FirstName = registrationRequest.FirstName!,
                LastName = registrationRequest.LastName!,
                Birthdate = registrationRequest.Birthdate,
            };

            var existUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email || u.Username == user.Username);

            if (existUser != null)
            {
                throw new AlreadyExistException();
            }

            try
            {
                var returnedUser = await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return returnedUser.Entity;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new Exception("An error occurred while registering the user");
            }
        }

        public async Task<User> UpdateUserAsync(int id, UpdateUserDTO updateUserDTO)
        {
            try
            {
                var existingUser = await FindUserByIdAsync(id);
                existingUser.Username = updateUserDTO.Username is null ? existingUser.Username : updateUserDTO.Username;
                existingUser.Birthdate = updateUserDTO.Birthdate is null ? existingUser.Birthdate : updateUserDTO.Birthdate;
                existingUser.FirstName = updateUserDTO.FirstName is null ? existingUser.FirstName : updateUserDTO.FirstName;
                existingUser.LastName = updateUserDTO.LastName is null ? existingUser.LastName : updateUserDTO.LastName;
                existingUser.Password = updateUserDTO.Password is null ? existingUser.Password : updateUserDTO.Password;

                await _context.SaveChangesAsync();
                return existingUser;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }

        public async Task<User> VerifyUserAsync(string email)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (existingUser is null) throw new NotFoundException("User not found");

            existingUser.IsVerified = true;

            await _context.SaveChangesAsync();

            return existingUser;
        }
    }
}