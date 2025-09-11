using MovieReservation.Server.Application.Common.Models;

namespace MovieReservation.Server.Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailDto emailDto, CancellationToken ct = default);
        string GenerateOtp();
    }
}