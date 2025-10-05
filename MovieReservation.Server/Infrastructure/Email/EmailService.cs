using MovieReservation.Server.Application.Common.Interfaces;
using MovieReservation.Server.Application.Common.Models;
using System.Net;
using System.Net.Mail;

namespace MovieReservation.Server.Infrastructure.Email;
public class EmailService : IEmailService
{
    // Dịch vụ triển khai email
    // Sứ dụng SMTP để gửi OTP xác nhận email
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
        // emailDto: thông tin người gửi
        // ct: token để hủy
    public async Task SendEmailAsync(EmailDto emailDto, CancellationToken ct = default)
    {
        // tạo smtpclient kết nối server
        var smtpClient = new SmtpClient(_configuration["EmailSettings:SmtpServer"])
        {
            Port = int.Parse(_configuration["EmailSettings:SmtpPort"]),
            Credentials = new NetworkCredential(
                _configuration["EmailSettings:Username"],
                _configuration["EmailSettings:Password"]),
            EnableSsl = true,
        };
        // tạo nội dung mail
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_configuration["EmailSettings:SenderEmail"],
                                 _configuration["EmailSettings:SenderName"]),
            Subject = emailDto.Subject,
            Body = emailDto.Body,
            IsBodyHtml = true,
        };

        //thêm người nhận
        mailMessage.To.Add(emailDto.ToEmail);

        //Gửi
        await smtpClient.SendMailAsync(mailMessage);
    }
    public string GenerateOtp()
    {
        // Create OTP 6 số random
        return new Random().Next(100000, 999999).ToString();
    }
}
