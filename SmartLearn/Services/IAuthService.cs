using SmartLearn.Models;

namespace SmartLearn.Services
{
    public interface IAuthService
    {
        Task<string?> RegisterAsync(User obj, string password);
        Task<string?> LoginAsync(string email, string password);
        Task<string?> GenerateJwtToken(User user);
    }
}
