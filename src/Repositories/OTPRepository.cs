using api.Data;
using api.Exceptions;
using api.Interfaces;
using api.Enities;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class OTPRepository : IOTPRepository
    {
        private readonly MongoContext _mongoContext;

        public OTPRepository(MongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }
        public async Task<OTPManager> CreateOtpAsync(OTPManager otpManager)
        {

            var newOtp = await _mongoContext.OTPManagers.AddAsync(otpManager);
            await _mongoContext.SaveChangesAsync();

            return newOtp.Entity;
        }

        public async Task<OTPManager> FindOtpByEmailAsync(string email)
        {
            var existingOtp = await _mongoContext.OTPManagers.FirstOrDefaultAsync(o => o.Email == email);

            if (existingOtp is null)
            {
                throw new NotFoundException("Otp not found");
            }

            return existingOtp;
        }

        public async Task<OTPManager> UpdateOtpAsync(OTPManager otpManager)
        {
            try
            {
                var otp = await FindOtpByEmailAsync(otpManager.Email);

                otp.OTP = otpManager.OTP;
                otp.CreatedAt = otpManager.CreatedAt;
                otp.ExpiredAt = otpManager.ExpiredAt;

                await _mongoContext.SaveChangesAsync();

                return otp;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}