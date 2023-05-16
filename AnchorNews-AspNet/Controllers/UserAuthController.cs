using AnchorNews.Models;
using AnchorNews_AspNet.Models;
using AnchorNews_AspNet.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class UserAuthController : ControllerBase
{
    private readonly UserService _userService;
    private readonly ILogger<UserAuthController> _logger;
    //private readonly IConfiguration _configuration;
    private IConfiguration Configuration { get; }
    public UserAuthController(UserService userService, ILogger<UserAuthController> logger, IConfiguration configuration)
    {
        _userService = userService;
        _logger = logger;
        Configuration  = configuration;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest model)
    {
        try
        {
            _userService.RegisterUser(model.Email, model.Password, model.FullName, model.Alias);
            return Ok("Registration successful.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during user registration.");
            return BadRequest("Registration failed.");
        }
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest model)
    {
        try
        {
            User user = _userService.Login(model.Email, model.Password);
            // Create claims for the user
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.UserType.ToString())
        };

            // Generate a secret key from the configuration
            var secretKey = Encoding.UTF8.GetBytes(Configuration["JwtSettings:SecretKey"]);

            // Create the token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(Configuration["JwtSettings:ExpirationMinutes"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };

            // Generate the token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            return Ok(new { Token = jwtToken });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during user login.");
            return BadRequest("Login failed.");
        }
    }
}
