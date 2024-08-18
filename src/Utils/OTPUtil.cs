namespace api.Utils
{
    public static class OTPUtil
    {
        public static string GenerateOtp()
        {
            Random random = new Random();
            int otp = random.Next(100000, 999999);
            return otp.ToString();
        }
    }
}
