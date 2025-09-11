using MovieReservation.Server.Application.Common.Interfaces;
using MovieReservation.Server.Application.Common.Models;
using System.Net;
using System.Net.Mail;

namespace MovieReservation.Server.Infrastructure.Email;
public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(EmailDto emailDto, CancellationToken ct = default)
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

        mailMessage.To.Add(emailDto.ToEmail);

        await smtpClient.SendMailAsync(mailMessage);
    }
    public string GenerateOtp()
    {
        return new Random().Next(100000, 999999).ToString();
    }
}