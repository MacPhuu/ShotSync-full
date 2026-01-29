using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoolBrackets_backend_dotnet.DTOs;
using PoolBrackets_backend_dotnet.Interfaces;
using PoolBrackets_backend_dotnet.Models;
using PoolBrackets_backend_dotnet.Models.Enums;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PoolBrackets_backend_dotnet.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize] // Mặc định yêu cầu Token cho mọi API (trừ những cái AllowAnonymous)
    public class UsersController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public UsersController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        #region 1. AUTHENTICATION (Login / Register)

        // POST: api/users/login
        [HttpPost("login")]
        [AllowAnonymous] // Ai cũng được truy cập
        public async Task<ActionResult<UserLoginDto>> Login([FromBody] LoginRequestDto loginRequest)
        {
            try
            {
                var result = await _authService.LoginAsync(loginRequest);
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Sai email hoặc mật khẩu." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/users/register
        [HttpPost("register")]
        [AllowAnonymous] // Ai cũng được truy cập
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            try
            {
                // Mapping DTO -> Entity (Thủ công)
                // Việc này ngăn chặn hacker tự gửi "Role: 1" để chiếm quyền Admin
                var newUser = new User
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    Password = dto.Password, // Service sẽ lo việc Hash password
                    Nation = dto.Nation,
                    Role = (int)UserRoleEnum.User, // Mặc định luôn là User (3)
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                var createdUser = await _authService.RegisterAsync(newUser);

                // Trả về kết quả (Ẩn mật khẩu)
                return Ok(new
                {
                    message = "Đăng ký thành công.",
                    userId = createdUser.Id,
                    email = createdUser.Email
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion

        #region 2. USER MANAGEMENT

        // GET: api/users
        // Quyền: Chỉ Admin (1) được xem danh sách tất cả user
        [HttpGet]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            var response = users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Nation = u.Nation,
                Avatar = u.Avatar,
                Role = u.Role,
                RoleName = ((UserRoleEnum)u.Role).ToString(),
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt
            });
            return Ok(response);
        }

        // GET: api/users/5
        // Quyền: Admin HOẶC Chính chủ
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUser(int id)
        {
            // Kiểm tra quyền sở hữu
            if (!IsAdminOrOwner(id)) return Forbid();

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound(new { message = "Người dùng không tồn tại." });

            var response = new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Nation = user.Nation,
                Avatar = user.Avatar,
                Role = user.Role,
                RoleName = ((UserRoleEnum)user.Role).ToString(),
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
            return Ok(response);
        }

        // PUT: api/users/5
        // Quyền: Admin HOẶC Chính chủ
        // Sử dụng UpdateUserDto để chỉ cho phép sửa Name, Nation, Avatar
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto dto)
        {
            // 1. Kiểm tra quyền
            if (!IsAdminOrOwner(id)) return Forbid();

            // 2. Tìm user cũ
            var existingUser = await _userService.GetUserByIdAsync(id);
            if (existingUser == null) return NotFound(new { message = "Người dùng không tồn tại." });

            // 3. Cập nhật dữ liệu an toàn
            existingUser.Name = dto.Name;
            existingUser.Nation = dto.Nation;

            if (!string.IsNullOrEmpty(dto.Avatar))
            {
                existingUser.Avatar = dto.Avatar;
            }

            // Lưu ý: Không cho phép sửa Email, Password, Role tại đây

            // 4. Lưu xuống DB
            await _userService.UpdateUserAsync(existingUser);

            return Ok(new { message = "Cập nhật thông tin thành công." });
        }

        // DELETE: api/users/5
        // Quyền: Chỉ Admin (1) được xóa user
        [HttpDelete("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound(new { message = "Người dùng không tồn tại." });

            await _userService.DeleteUserAsync(id);
            return NoContent();
        }

        #endregion

        #region 3. HELPER FUNCTIONS

        // Kiểm tra xem User hiện tại có phải Admin (Role 1) không
        private bool IsAdmin()
        {
            var roleClaim = User.FindFirst(ClaimTypes.Role);
            // So sánh với "1" (UserRoleEnum.Admin)
            return roleClaim != null && roleClaim.Value == ((int)UserRoleEnum.Admin).ToString();
        }

        // Kiểm tra quyền: Là Admin HOẶC là chủ sở hữu ID tài khoản này
        private bool IsAdminOrOwner(int resourceOwnerId)
        {
            if (IsAdmin()) return true;

            var userIdClaim = User.FindFirst("id"); // Lấy ID từ Token JWT
            if (userIdClaim == null) return false;

            // So sánh ID trong Token với ID cần truy cập
            return int.Parse(userIdClaim.Value) == resourceOwnerId;
        }

        #endregion
    }
}