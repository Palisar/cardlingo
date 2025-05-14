using FlipCardApp.Domain;

namespace FlipCardApp.Application.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Token)> Login(string username, string password);
        Task<(bool Success, string Message)> Register(User user, string password);
    }
}
