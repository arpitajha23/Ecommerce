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

                var mail = new MailMessage();
                mail.From = new MailAddress(smtpSettings["FromEmail"], smtpSettings["FromName"]);
                mail.To.Add(toEmail);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (var smtp = new SmtpClient(smtpSettings["Host"], int.Parse(smtpSettings["Port"])))
                {
                    smtp.Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]);
                    smtp.EnableSsl = bool.Parse(smtpSettings["EnableSSL"]);
                    smtp.Send(mail);
                }
                return true;
            }
            catch
            {
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
            <p>Hi {fullName},</p>
            <p>Welcome to QuickCart! We're excited to have you on board.</p>
            <p>Start exploring your dashboard now.</p>
            <p>Thanks, <br/> QuickCart Team</p>";
        }
    }
}
