namespace AnchorNews_AspNet.Models.Auth
{
    public class UserRoleRequest
    {
        public string UserId { get; set; }
        public string RoleName { get; set; } = "Guest";
    }
}
