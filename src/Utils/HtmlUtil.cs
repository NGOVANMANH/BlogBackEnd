namespace api.Utils;

public static class HtmlUtil
{
    public static string GetOTPEmail(string OTP)
    {
        return $@"
        <!DOCTYPE html>
        <html lang='en'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>OTP Verification</title>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f4f4f4;
                    margin: 0;
                    padding: 20px;
                }}
                .container {{
                    max-width: 600px;
                    margin: 0 auto;
                    padding: 20px;
                    background-color: #ffffff;
                    border-radius: 5px;
                    box-shadow: 0 2px 4px rgba(0,0,0,0.1);
                }}
                .header {{
                    text-align: center;
                    margin-bottom: 20px;
                }}
                .otp {{
                    font-size: 24px;
                    font-weight: bold;
                    color: #333;
                    text-align: center;
                    margin: 20px 0;
                }}
                .footer {{
                    text-align: center;
                    margin-top: 20px;
                    font-size: 12px;
                    color: #777;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='header'>
                    <h2>OTP Verification</h2>
                </div>
                <p>Dear user,</p>
                <p>Thank you for choosing our service. Your One-Time Password (OTP) for verification is:</p>
                <div class='otp'>{OTP}</div>
                <p>Please enter this OTP to complete your verification process. This OTP is valid for the next 5 minutes.</p>
                <p>If you did not request this, please ignore this email.</p>
                <div class='footer'>
                    <p>Best regards,</p>
                    <p>Your Company Name</p>
                </div>
            </div>
        </body>
        </html>
    ";
    }

    public static string GetVerificationResultPage(string status = "")
    {
        string template = File.ReadAllText("./Utils/html/VerificationStatus.html");
        string statusIcon, statusMessage, statusDetail, statusClass;

        switch (status.ToLower())
        {
            case "success":
                statusIcon = "😊";
                statusMessage = "Verification Successful!";
                statusDetail = "Your email has been successfully verified.";
                statusClass = "status-success";
                break;
            case "expired":
                statusIcon = "😞";
                statusMessage = "Verification Failed!";
                statusDetail = "The token has expired. Please request a new one.";
                statusClass = "status-failure";
                break;
            case "invalid":
                statusIcon = "😞";
                statusMessage = "Verification Failed!";
                statusDetail = "The token is invalid. Please ensure you have the correct token.";
                statusClass = "status-failure";
                break;
            case "notfound":
                statusIcon = "😞";
                statusMessage = "Verification Failed!";
                statusDetail = "The account associated with this token does not exist.";
                statusClass = "status-failure";
                break;
            case "servererror":
                statusIcon = "😞";
                statusMessage = "Verification Failed!";
                statusDetail = "An internal server error occurred. Please try again later.";
                statusClass = "status-failure";
                break;
            default:
                statusIcon = "😞";
                statusMessage = "Verification Failed!";
                statusDetail = "An unknown error occurred. Please contact support.";
                statusClass = "status-failure";
                break;
        }

        template = template.Replace("[StatusIcon]", statusIcon)
                           .Replace("[StatusMessage]", statusMessage)
                           .Replace("[StatusDetail]", statusDetail)
                           .Replace("[StatusClass]", statusClass);

        return template;
    }

    public static string GetVerificationEmail(string verificationLink)
    {
        string verificationEmail = $@"
                <html lang=""en"">
                <head>
                    <meta charset=""UTF-8"" />
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
                    <title>Email Verification</title>
                </head>
                <body>
                    <h2>Email Verification</h2>
                    <p>Hello</p>
                    <p>
                    Thank you for registering with us. Please verify your email address by
                    clicking the link below:
                    </p>
                    <p><a href=""{verificationLink}"" target=""_blank"">Verify Email</a></p>
                    <p>If you did not request this email, please ignore it.</p>
                    <p>Best regards,</p>
                    <p>Your Company Name</p>
                </body>
                </html>";

        ;

        return verificationEmail;
    }
}