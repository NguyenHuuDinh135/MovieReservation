using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Application.Common.Models;
namespace MovieReservation.Server.Infrastructure.Identity
{
    public class IdentityService
    {
        // Biến thành viên
        private readonly UserManager<User> _userManager;
        private readonly IUserClaimsPrincipalFactory<User> _userClaimsPrincipalFactory;
        private readonly IAuthorizationService _authorizationService;
        // Hàm khởi tạo
        public IdentityService(
            UserManager<User> userManager,
            IUserClaimsPrincipalFactory<User> userClaimsPrincipalFactory,
            IAuthorizationService authorizationService)
        {
            _userManager = userManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _authorizationService = authorizationService;
        }
        // Phương thức thành viên công khai bất đồng bộ để lấy tên người dùng theo ID người dùng
        public async Task<string?> GetUserNameAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return user?.UserName;
        }
        // Phương thức thành viên công khai bất đồng bộ để tạo người dùng mới
        public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
        {
            var user = new User
            {
                UserName = userName,
                Email = userName,
            };

            var result = await _userManager.CreateAsync(user, password);

            return (result.ToApplicationResult(), user.Id);
        }
        // Phương thức thành viên công khai bất đồng bộ để kiểm tra xem người dùng có thuộc vai trò cụ thể hay không
        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return user != null && await _userManager.IsInRoleAsync(user, role);
        }
        // Phương thức thành viên công khai bất đồng bộ để ủy quyền người dùng dựa trên chính sách
        public async Task<bool> AuthorizeAsync(string userId, string policyName)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

            var result = await _authorizationService.AuthorizeAsync(principal, policyName);

            return result.Succeeded;
        }
        // Phương thức thành viên công khai bất đồng bộ để xóa người dùng theo ID người dùng
        public async Task<Result> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return user != null ? await DeleteUserAsync(user) : Result.Success();
        }
        // Phương thức thành viên công khai bất đồng bộ để xóa người dùng
        public async Task<Result> DeleteUserAsync(User user)
        {
            var result = await _userManager.DeleteAsync(user);

            return result.ToApplicationResult();
        }
    }
}