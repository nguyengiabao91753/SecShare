using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.Helper.EmailHelper
{
    public class EmailHelper : IEmailHelper
    {
        private readonly EmailSetting _emailSetting;

        public EmailHelper(IOptions<EmailSetting> emailSetting)
        {
            _emailSetting = new EmailSetting
            {
                SmtpServer = "smtp.gmail.com",
                Port=587,
                SenderName = "SecShare System",
                SenderEmail="mailsendotp0704@gmail.com",
                Password="rgrzrrlcndqsaurb"
            };
        }

        public async Task<bool> SendEmailAsync(string toEmail, string otp)
        {
            try
            {
                var message = new MailMessage
                {
                    From = new MailAddress(_emailSetting.SenderEmail, _emailSetting.SenderName),
                    Subject = "Your SecShare OTP Verification code",
                    Body = $"Your OTP code is {otp}</br> This code will be expire in 30 minutes.",
                    IsBodyHtml = true
                };

                message.To.Add(new MailAddress(toEmail));

                using var smtp = new SmtpClient(_emailSetting.SmtpServer, _emailSetting.Port)
                {
                    Credentials = new NetworkCredential(_emailSetting.SenderEmail, _emailSetting.Password),
                    EnableSsl = true
                };

                await smtp.SendMailAsync(message);
                return true;
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine($"SMTP Error: {smtpEx.StatusCode} - {smtpEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex}");
                return false;
            }
        }
    }
}
