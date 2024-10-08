using api.Exceptions;
using api.Interfaces;
using api.Enities;
using api.Utils;

namespace api.Services
{
    public class OTPService : IOTPService
    {
        private readonly IOTPRepository _otpRepository;

        public OTPService(IOTPRepository otpRepository)
        {
            _otpRepository = otpRepository;
        }
        public async Task<string> GenerateOtpAsync(string email)
        {
            var otp = OTPUtil.GenerateOtp();

            try
            {
                var existingOtp = await _otpRepository.FindOtpByEmailAsync(email);

                existingOtp.OTP = otp;
                existingOtp.CreatedAt = DateTime.UtcNow;
                existingOtp.ExpiredAt = DateTime.UtcNow.AddMinutes(5);

                var newOtp = await _otpRepository.UpdateOtpAsync(existingOtp);

                return newOtp.OTP;
            }
            catch (NotFoundException)
            {
                var newOtpManger = new OTPManager
                {
                    Email = email,
                    OTP = otp,
                    CreatedAt = DateTime.UtcNow,
                    ExpiredAt = DateTime.UtcNow.AddMinutes(5),
                };

                var newOtp = await _otpRepository.CreateOtpAsync(newOtpManger);

                return newOtp.OTP;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OTPManager?> VerifyOtpAsync(string email, string OTP)
        {
            var existingOtp = await _otpRepository.FindOtpByEmailAsync(email);

            if (existingOtp.ExpiredAt < DateTime.UtcNow)
            {
                throw new ExpiredException("OTP is expired");
            }

            if (existingOtp.OTP != OTP)
            {
                return null;
            }

            return existingOtp;
        }
    }

}