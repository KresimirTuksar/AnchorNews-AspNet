
namespace AnchorNews.Models
{
    public class LoginResponse
    {

        public string UserId { get; set; } = null;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public List<string> Roles { get; set; } = null!;
        public string Token { get; set; } = null!;

        public DateTime Expiration { get; set; }
    }
}
