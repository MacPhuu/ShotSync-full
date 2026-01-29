using System.ComponentModel.DataAnnotations;
using PoolBrackets_backend_dotnet.Models.Enums;

namespace PoolBrackets_backend_dotnet.DTOs
{
    // 1. DTO NHẬN DỮ LIỆU ĐĂNG NHẬP (Input)
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        public string Password { get; set; } = string.Empty;
    }

    // 2. DTO NHẬN DỮ LIỆU ĐĂNG KÝ (Input)
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "Tên là bắt buộc")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6, ErrorMessage = "Mật khẩu phải từ 6 ký tự trở lên")]
        public string Password { get; set; } = string.Empty;

        public string? Nation { get; set; }
    }

    // 3. DTO NHẬN DỮ LIỆU CẬP NHẬT (Input - Không cho sửa Role/Password)
    public class UpdateUserDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Nation { get; set; }
        public string? Avatar { get; set; }
    }
    
    // 4. DTO TRẢ VỀ KHI LOGIN THÀNH CÔNG (Output - Của bạn đang có)
    // Tôi đã bổ sung thêm Id để Frontend tiện sử dụng
    public class UserLoginDto
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        
        public required string Role { get; set; } 
        
        public required string Token { get; set; }
    }
}