using api.Data;
using api.DTOs;
using api.Exceptions;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Interfaces
{
    public interface IVerifyInformationRepository
    {
        Task<VerifyInformation> CreateVerifyInformationAsync(VerifyInformation verifyInformation);
        Task<Boolean> VerifyInformationAsync(EmailConfirmationDTO emailConfirmationDTO);
    }
}

namespace api.Repositories
{
    public class VerifyInformationRepository : IVerifyInformationRepository
    {
        private readonly BlogDbContext _context;

        public VerifyInformationRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<VerifyInformation> CreateVerifyInformationAsync(VerifyInformation verifyInformation)
        {
            try
            {
                var existingVerifyInformation = await _context.VerifyInformations.FirstOrDefaultAsync(v => v.Email == verifyInformation.Email);
                if (existingVerifyInformation != null)
                {
                    _context.VerifyInformations.Remove(existingVerifyInformation);
                }
                await _context.VerifyInformations.AddAsync(verifyInformation);
                await _context.SaveChangesAsync();
                return verifyInformation;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<Boolean> VerifyInformationAsync(EmailConfirmationDTO emailConfirmationDTO)
        {
            try
            {
                var existingVerifyInformation = await _context.VerifyInformations
                    .FirstOrDefaultAsync(v => v.Email == emailConfirmationDTO.Email && v.VerifyToken == emailConfirmationDTO.VerifyToken);

                if (existingVerifyInformation == null)
                {
                    throw new VerifyTokenInvalidException();
                }

                if (existingVerifyInformation.VerifyTokenExpiry < DateTime.UtcNow)
                {
                    throw new VerifyTokenExpiredException();
                }

                existingVerifyInformation.VerifyToken = null;
                existingVerifyInformation.VerifyTokenExpiry = null;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}