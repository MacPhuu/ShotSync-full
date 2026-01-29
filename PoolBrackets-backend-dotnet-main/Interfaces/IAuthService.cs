using System.Threading.Tasks;
using PoolBrackets_backend_dotnet.DTOs;
using PoolBrackets_backend_dotnet.Models;

namespace PoolBrackets_backend_dotnet.Interfaces
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(User user);
        
        Task<UserLoginDto> LoginAsync(LoginRequestDto loginRequest); 
    }
}