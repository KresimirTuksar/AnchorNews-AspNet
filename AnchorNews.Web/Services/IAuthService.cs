using AnchorNews.Models;

namespace AnchorNews.Web.Services
{
    public interface IAuthService
    {
        Task Login(LoginRequest loginRequest);
        Task Register(RegisterRequest registerRequest);
        Task Logout();
    }
}
