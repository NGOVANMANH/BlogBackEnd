using api.Data;
using api.DTOs;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;
using api.Utils;
using api.Exceptions;

namespace api.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> LoginUserAsync(LoginDTO loginDTO);
        public Task<User> RegisterUserAsync(RegisterDTO registerDTO);
        public Task SetUserVerificationAsync(String Email, bool IsVerified);
        public Task<User> UpdateUserAsync(User user);
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
        public async Task<User> LoginUserAsync(LoginDTO login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == login.Email);
            if (user == null || BcryptUtil.VerifyPassword(login.Password!, user.Password) == false)
            {
                throw new UserNotExistException();
            }
            else if (user.IsVerified == false)
            {
                throw new UserNotVerifiedException();
            }
            return user;
        }

        public async Task<User> RegisterUserAsync(RegisterDTO registerDTO)
        {
            var hashedPassword = BcryptUtil.HashPassword(registerDTO.Password!);
            var user = new User
            {
                Email = registerDTO.Email!,
                Username = registerDTO.Username!,
                Password = hashedPassword,
                FirstName = registerDTO.FirstName!,
                LastName = registerDTO.LastName!,
                Birthdate = registerDTO.Birthdate,
            };

            var existUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email || u.Username == user.Username);

            if (existUser != null)
            {
                throw new UserAlreadyExistException();
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

        public async Task SetUserVerificationAsync(String Email, bool IsVerified)
        {
            try
            {
                var existUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);
                if (existUser == null)
                {
                    throw new UserNotExistException();
                }
                else
                {
                    existUser.IsVerified = IsVerified;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new Exception("An error occurred while verifying the user");
            }
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            try
            {
                var existUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (existUser == null)
                {
                    throw new UserNotExistException();
                }
                else
                {
                    existUser.Username = user.Username;
                    existUser.FirstName = user.FirstName;
                    existUser.LastName = user.LastName;
                    existUser.Birthdate = user.Birthdate;
                    existUser.Password = BcryptUtil.HashPassword(user.Password);
                    await _context.SaveChangesAsync();
                    return existUser;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new Exception("An error occurred while updating the user");
            }
        }
    }
}