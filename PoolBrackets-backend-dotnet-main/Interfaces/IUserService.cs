using PoolBrackets_backend_dotnet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoolBrackets_backend_dotnet.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
    }
}
