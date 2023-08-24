using System.ComponentModel.DataAnnotations;

namespace AnchorNews_AspNet.Models.Auth
{
    public class AuthRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
