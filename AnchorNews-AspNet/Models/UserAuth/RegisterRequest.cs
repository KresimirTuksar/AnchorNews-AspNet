﻿namespace AnchorNews_AspNet.Models.UserAuth
{
    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Alias { get; set; }
    }
}
