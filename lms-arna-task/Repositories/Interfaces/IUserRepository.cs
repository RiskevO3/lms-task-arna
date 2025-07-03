using lms_arna_task.Models;
using System.Threading.Tasks;

namespace lms_arna_task.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User> AddUserAsync(User user);
        Task<User?> GetUserByIdAsync(int id);
        Task<List<User>> GetSubordinatesAsync(int managerId);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
        Task<List<User>> GetAllUsersAsync();
    }
}
