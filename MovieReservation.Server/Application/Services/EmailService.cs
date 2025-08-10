using MovieReservation.Server.Application.Dtos;
using System.Net;
using System.Net.Mail;

namespace MovieReservation.Server.Application.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(EmailDto emailDto)
        {
            var smtpClient = new SmtpClient(_configuration["EmailSettings:SmtpServer"])
            {
                Port = int.Parse(_configuration["EmailSettings:SmtpPort"]),
                Credentials = new NetworkCredential(
                    _configuration["EmailSettings:Username"],
                    _configuration["EmailSettings:Password"]),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["EmailSettings:SenderEmail"],
                                     _configuration["EmailSettings:SenderName"]),
                Subject = emailDto.Subject,
                Body = emailDto.Body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(emailDto.To);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}