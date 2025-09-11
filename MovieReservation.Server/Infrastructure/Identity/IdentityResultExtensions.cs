using Microsoft.AspNetCore.Identity;
using MovieReservation.Server.Application.Common.Models;
namespace MovieReservation.Server.Infrastructure.Identity
{
    // Tiện ích mở rộng cho IdentityResult
    public static class IdentityResultExtensions
    {
        // Phương thức mở rộng để chuyển đổi IdentityResult thành Result của ứng dụng
        public static Result ToApplicationResult(this IdentityResult result)
        {
            // Nếu thành công, trả về Result thành công, ngược lại trả về Result thất bại với các mô tả lỗi
            return result.Succeeded
            ? Result.Success()
            : Result.Failure(result.Errors.Select(e => e.Description));
        }
    }
}