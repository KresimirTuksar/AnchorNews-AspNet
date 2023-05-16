using Microsoft.AspNetCore.Identity;

namespace AnchorNews.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public string Alias { get; set; }
        public UserType UserType { get; set; }
    }

    public enum UserType
    {
        Admin,
        Editor,
        Guest
    }
}