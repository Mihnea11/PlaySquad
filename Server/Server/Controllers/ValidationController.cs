using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Models.Requests;
using Server.Models.Responses;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ValidationController : ControllerBase
    {
        private readonly IUserService _userService;

        public ValidationController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("validate-user")]
        public async Task<IActionResult> ValidateUser([FromBody] UserValidationRequest request)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(request.Email);

                if (user.Id == request.Id && user.Name == request.Name)
                {
                    return Ok(new
                    {
                        Message = "User validation successful",
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

                return Unauthorized(new { Message = "User validation failed: Data mismatch" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
