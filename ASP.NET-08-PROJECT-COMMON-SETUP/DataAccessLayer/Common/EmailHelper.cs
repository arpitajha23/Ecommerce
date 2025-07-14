using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Common
{
    public class EmailHelper
    {
        private readonly IConfiguration _config;


        public EmailHelper(IConfiguration config)
        {
            _config = config;
        }

        public bool SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                var smtpSettings = _config.GetSection("SMTPSettings");

                string fromEmail = smtpSettings["FromEmail"];
                string fromName = smtpSettings["FromName"] ?? "QuickCart";
                string host = smtpSettings["Host"];
                int port = int.TryParse(smtpSettings["Port"], out var parsedPort) ? parsedPort : 587;
                string username = smtpSettings["Username"];
                string password = smtpSettings["Password"];
                bool enableSSL = bool.TryParse(smtpSettings["EnableSSL"], out var ssl) ? ssl : true;

                var mail = new MailMessage
                {
                    From = new MailAddress(fromEmail, fromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mail.To.Add(toEmail);

                using var smtp = new SmtpClient(host, port)
                {
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = enableSSL
                };

                smtp.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                // Optional: Log error if you use a logger
                Console.WriteLine($"Email send error: {ex.Message}");
                return false;
            }
        }

        public static string GetForgotPasswordBody(string resetLink)
        {
            return $@"
            <p>Hi,</p>
            <p>You requested to reset your password.</p>
            <p><a href='{resetLink}'>Click here to reset your password</a></p>
            <p>If you did not request this, please ignore this email.</p>
            <p>Thanks, <br/> QuickCart Team</p>";
        }

        public static string GetEmailVerificationBody(string verificationLink)
        {
            return $@"
            <p>Hi,</p>
            <p>Thank you for registering. Please verify your email address by clicking the link below:</p>
            <p><a href='{verificationLink}'>Verify Email</a></p>
            <p>If you did not sign up, you can safely ignore this email.</p>
            <p>Thanks, <br/> QuickCart Team</p>";
        }

        public static string GetWelcomeBody(string fullName)
        {
            return $@"
    <!DOCTYPE html>
    <html>
    <head>
        <style>
            .email-container {{
                font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                background-color: #f4f4f4;
                padding: 30px;
            }}
            .email-content {{
                max-width: 600px;
                margin: 0 auto;
                background-color: #ffffff;
                border-radius: 8px;
                box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
                overflow: hidden;
            }}
            .header {{
                background-color: #4CAF50;
                color: white;
                padding: 20px;
                text-align: center;
                font-size: 24px;
            }}
            .body {{
                padding: 30px;
                color: #333;
            }}
            .body p {{
                font-size: 16px;
                line-height: 1.6;
            }}
            .cta {{
                display: inline-block;
                margin-top: 20px;
                padding: 12px 24px;
                background-color: #4CAF50;
                color: white;
                text-decoration: none;
                border-radius: 6px;
                font-weight: bold;
                transition: background-color 0.3s;
            }}
            .cta:hover {{
                background-color: #45a049;
            }}
            .footer {{
                padding: 20px;
                text-align: center;
                font-size: 14px;
                color: #777;
                background-color: #f0f0f0;
            }}
        </style>
    </head>
    <body>
        <div class='email-container'>
            <div class='email-content'>
                <div class='header'>
                    Welcome to QuickCart 🎉
                </div>
                <div class='body'>
                    <p>Hi <strong>{fullName}</strong>,</p>
                    <p>We’re thrilled to have you onboard with <strong>QuickCart</strong>! Your account has been created successfully.</p>
                    <p>Start exploring your dashboard, discover amazing deals, and enjoy smart shopping experiences!</p>
                    <a class='cta' href='http://localhost:4200/dashboard'>Go to Dashboard</a>
                    <p>Thanks,<br/>The QuickCart Team</p>
                </div>
                <div class='footer'>
                    © {DateTime.Now.Year} QuickCart. All rights reserved.
                </div>
            </div>
        </div>
    </body>
    </html>
    ";
        }

    }
}
