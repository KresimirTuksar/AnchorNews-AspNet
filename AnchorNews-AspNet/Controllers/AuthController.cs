using AnchorNews_AspNet.Data;
using AnchorNews_AspNet.Models.Auth;
using AnchorNews_AspNet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AnchorNews_AspNet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly UsersDbContext _context;

        private readonly TokenService _tokenService;
        public AuthController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, UsersDbContext context, TokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;

            _context = context;

            _tokenService = tokenService;
        }
        [HttpPost]
        [Route("register")]
        [AllowAnonymous]

        public async Task<IActionResult> Register(RegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userManager.CreateAsync(
                new IdentityUser { UserName = request.Username, Email = request.Email },
                request.Password
            );
            if (result.Succeeded)
            {
                request.Password = "";
                return CreatedAtAction(nameof(Register), new { email = request.Email }, request);
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var managedUser = await _userManager.FindByEmailAsync(request.Email);
            if (managedUser == null)
            {
                return BadRequest("User does not exist");
            }
            var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password);
            if (!isPasswordValid)
            {
                return BadRequest("Invalid password");
            }
            var userInDb = _context.Users.FirstOrDefault(u => u.Email == request.Email);
            if (userInDb is null)
                return Unauthorized();

            var roles = await _userManager.GetRolesAsync(userInDb);


            var accessToken = _tokenService.CreateToken(userInDb, roles);
            await _context.SaveChangesAsync();
            return Ok(new AuthResponse
            {
                UserId = userInDb.Id,
                Username = userInDb.UserName,
                Email = userInDb.Email,
                Roles = (List<string>)roles,
                Token = accessToken,
                Expiration = DateTime.UtcNow.AddHours(1)
            }); ;
        }



        // FOR TESTING ONLY
        [HttpPost]
        [Route("createRole")]
        [AllowAnonymous]

        public async Task<IActionResult> CreateRole(RoleRequest request)
        {
            var role = new IdentityRole { Name = request.Name }; // Admin, Editor, Guest
            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                // Role creation successful
                return Ok();
            }
            else
            {
                // Role creation failed, handle the error
                var errors = result.Errors;
                return BadRequest(errors);
            }
        }

        [HttpPost]
        [Route("assignRole")]
        [AllowAnonymous]

        public async Task<IActionResult> AssignRole(UserRoleRequest request)
        {
            // Find the user by their ID
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                // Handle the case where the user is not found
                return NotFound();
            }

            // Check if the role already exists
            var role = await _roleManager.FindByNameAsync(request.RoleName);
            if (role == null)
            {
                // Handle the case where the role does not exist
                return NotFound();
            }

            // Assign the role to the user
            var result = await _userManager.AddToRoleAsync(user, role.Name);
            if (result.Succeeded)
            {
                // Role assignment successful
                return Ok(result);
            }
            else
            {
                // Handle the case where the role assignment failed
                // You can inspect the 'result.Errors' property for the specific errors
                return BadRequest(result.Errors);
            }
        }

    }
}
