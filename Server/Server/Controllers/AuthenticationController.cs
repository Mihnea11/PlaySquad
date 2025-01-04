using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Models.Entities;
using Server.Models.Requests;
using Server.Models.Responses;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IGoogleTokenValidator _googleTokenValidator;
        private readonly IRoleService _roleService;

        public AuthenticationController(IUserService userService, IGoogleTokenValidator googleTokenValidator, IRoleService roleService)
        {
            _userService = userService;
            _googleTokenValidator = googleTokenValidator;
            _roleService = roleService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> NormalLogin([FromBody] LoginRequest request)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(request.Email);

                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    return Unauthorized(new { Message = "Invalid email or password" });
                }

                return Ok(new
                {
                    Message = "Login successful",
                    User = new UserResponse
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Name = user.Name,
                        PictureUrl = user.PictureUrl,
                        RoleName = user.Role?.Name
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            try
            {
                var payload = await _googleTokenValidator.ValidateGoogleTokenAsync(request.IdToken);

                User? user = null;
                try
                {
                    user = await _userService.GetUserByEmailAsync(payload.Email);
                }
                catch (Exception ex)
                {
                    if (ex.Message == "User not found")
                    {
                        var defaultRole = await _roleService.GetDefaultRoleAsync();
                        if (defaultRole == null)
                        {
                            throw new Exception("Default role not found.");
                        }

                        user = new User
                        {
                            Email = payload.Email,
                            Name = payload.Name,
                            PictureUrl = payload.Picture,
                            GoogleId = payload.Subject,
                            RoleId = defaultRole.Id
                        };

                        user = await _userService.CreateUserAsync(user);
                    }
                    else
                    {
                        throw;
                    }
                }

                if (user.GoogleId != payload.Subject)
                {
                    user.GoogleId = payload.Subject;
                    await _userService.UpdateUserAsync(user.Id, user);
                }

                return Ok(new
                {
                    Message = "Google login successful",
                    User = new UserResponse
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Name = user.Name,
                        PictureUrl = user.PictureUrl,
                        RoleName = user.Role?.Name
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var role = await _roleService.GetDefaultRoleAsync();
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

                var user = new User
                {
                    Email = request.Email,
                    PasswordHash = hashedPassword,
                    Name = request.Name,
                    RoleId = role.Id
                };

                var createdUser = await _userService.CreateUserAsync(user);

                return CreatedAtAction(nameof(Register), new
                {
                    Message = "User registered successfully.",
                    User = new UserResponse
                    {
                        Id = createdUser.Id,
                        Email = createdUser.Email,
                        Name = createdUser.Name,
                        PictureUrl = createdUser.PictureUrl,
                        RoleName = role.Name,
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
