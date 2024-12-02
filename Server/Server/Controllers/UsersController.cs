using Microsoft.AspNetCore.Mvc;
using Server.Repositories;
using Server.Models;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UsersController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userRepository.GetUsersAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            var result = await _userRepository.AddUserAsync(user);
            return result > 0 ? Ok(new { Message = "User added successfully" }) : BadRequest(new { Message = "Error adding user" });
        }
    }


}
