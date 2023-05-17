using AnchorNews.Data;
using AnchorNews_AspNet.Models.UserAuth;
using System;

namespace AnchorNews_AspNet.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _dbContext;

        public UserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void RegisterUser(string email, string password, string fullName, string alias)
        {
            // Check if the email is already taken
            if (_dbContext.Users.Any(u => u.Email == email))
            {
                throw new Exception("Email is already registered.");
            }

            // Hash the password
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            // Create a new User object
            User user = new User
            {
                Email = email,
                PasswordHash = passwordHash,
                FullName = fullName,
                Alias = alias,
                UserType = UserType.Guest //assuming all new users are created as guests
            };

            // Save the user to the database
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }

        public User Login(string email, string password)
        {
            // Retrieve the user from the database based on the email
            User user = _dbContext.Users.FirstOrDefault(u => u.Email == email);

            // Verify the password
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            if (!isPasswordValid)
            {
                throw new Exception("Invalid email or password.");
            }

            return user;
        }
    }
}
