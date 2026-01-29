using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens; // Dùng cho SymmetricSecurityKey
using PoolBrackets_backend_dotnet.DTOs;
using PoolBrackets_backend_dotnet.Interfaces;
using PoolBrackets_backend_dotnet.Models;
using PoolBrackets_backend_dotnet.Models.Enums;
using System;
using System.IdentityModel.Tokens.Jwt; // Dùng cho JwtSecurityToken
using System.Security.Claims; // Dùng cho Claim, ClaimTypes
using System.Text;
using System.Threading.Tasks;

namespace PoolBrackets_backend_dotnet.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IPlayerRepository playerRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _playerRepository = playerRepository;
            _configuration = configuration;
        }

        public async Task<User> RegisterAsync(User user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            // Sửa lỗi CS0117: Giờ UserRoleEnum đã có .User
            if (user.Role == 0)
            {
                user.Role = (int)UserRoleEnum.User;
            }

            await _userRepository.AddUserAsync(user);

            // Auto-create Player profile
            var newPlayer = new Player
            {
                Name = user.Name,
                Email = user.Email,
                UserId = user.Id,
                Nation = user.Nation ?? "Unknown",
                IsActive = true,
                CreatedAt = DateTime.Now,
                Portrait = user.Avatar 
            };

            await _playerRepository.AddPlayerAsync(newPlayer);

            return user;
        }

        public async Task<UserLoginDto> LoginAsync(LoginRequestDto loginRequest)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginRequest.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
            {
                throw new UnauthorizedAccessException("Email hoặc mật khẩu không chính xác.");
            }

            var token = GenerateJwtToken(user);

            return new UserLoginDto
            {
                Id = user.Id,
                UserName = user.Name, // Sửa lỗi CS1061: Giờ Model User đã có .Name
                Email = user.Email,
                Role = ((UserRoleEnum)user.Role).ToString(),
                Token = token
            };
        }

        private string GenerateJwtToken(User user)
        {
            // Sửa lỗi CS1503: Dùng ClaimTypes.Name thay vì JwtRegisteredClaimNames.Sub để tránh xung đột
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Name), // Lưu tên hiển thị
                new Claim(ClaimTypes.Email, user.Email), // Lưu email
                
                // Role: Ép kiểu sang string ("1", "2", "3")
                new Claim(ClaimTypes.Role, user.Role.ToString()),

                new Claim("id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Sửa Warning CS8604: Xử lý trường hợp Key bị null bằng toán tử ??
            var keyString = _configuration["Jwt:Key"] ?? "Key_Mac_Dinh_Dai_Hon_32_Ky_Tu_Neu_Config_Loi";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Sửa Warning CS8604 cho ExpireMinutes
            var expireMinutes = Convert.ToDouble(_configuration["Jwt:ExpireMinutes"] ?? "60");

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(expireMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}