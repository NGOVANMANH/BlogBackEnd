namespace api.Utils;

public static class HtmlTemplate
{
    public static string ThanksForConfirmingEmail = @"
        <!DOCTYPE html>
        <html lang='en'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>Thank You</title>
            <style>
                body {
                    font-family: Arial, sans-serif;
                    background-color: #f4f4f4;
                    color: #333;
                    margin: 0;
                    padding: 20px;
                    text-align: center;
                }
                .container {
                    background-color: #ffffff;
                    padding: 20px;
                    border-radius: 5px;
                    box-shadow: 0 2px 4px rgba(0,0,0,0.1);
                    display: inline-block;
                    margin-top: 50px;
                }
                h1 {
                    color: #007bff;
                }
            </style>
        </head>
        <body>
            <div class='container'>
                <h1>Thank You for Verifying Your Email</h1>
                <p>Your email has been successfully verified.</p>
            </div>
        </body>
        </html>";
    public static string GetEmailVerificationTemplate(string verificationUrl)
    {
        return $@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Verify Your Email</title>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f4f4f4;
                            color: #333;
                            margin: 0;
                            padding: 20px;
                        }}
                        .email-container {{
                            background-color: #ffffff;
                            padding: 20px;
                            border-radius: 5px;
                            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
                        }}
                        .email-header {{
                            text-align: center;
                            padding-bottom: 20px;
                        }}
                        .email-body {{
                            padding: 10px 20px;
                        }}
                        .email-footer {{
                            text-align: center;
                            padding-top: 20px;
                            color: #888;
                            font-size: 12px;
                        }}
                        .verify-button {{
                            display: inline-block;
                            padding: 10px 20px;
                            margin-top: 20px;
                            background-color: #007bff;
                            color: #ffffff;
                            text-decoration: none;
                            border-radius: 5px;
                        }}
                    </style>
                </head>
                <body>
                    <div class='email-container'>
                        <div class='email-header'>
                            <h1>Xác Thực Email Của Bạn</h1>
                        </div>
                        <div class='email-body'>
                            <p>Chào bạn,</p>
                            <p>Cảm ơn bạn đã đăng ký tài khoản tại ứng dụng của chúng tôi. Vui lòng nhấp vào liên kết dưới đây để xác thực địa chỉ email của bạn:</p>
                            <a href='{verificationUrl}' class='verify-button'>Xác Thực Email</a>
                            <p>Nếu bạn không đăng ký tài khoản này, vui lòng bỏ qua email này.</p>
                            <p>Trân trọng,</p>
                            <p>Đội ngũ Hỗ trợ</p>
                        </div>
                        <div class='email-footer'>
                            <p>&copy; 2024 Ứng Dụng của Chúng Tôi. Mọi quyền được bảo lưu.</p>
                        </div>
                    </div>
                </body>
                </html>";
    }
}