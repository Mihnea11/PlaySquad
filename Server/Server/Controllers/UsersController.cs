using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Repositories;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserRepository _repository;

    public UsersController(UserRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        return Ok(await _repository.GetAllAsync("Users"));
    }

    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody] User user)
    {
        var result = await _repository.AddUserAsync(user);
        return result > 0 ? Ok(new { Message = "User added successfully" }) : BadRequest(new { Message = "Error adding user" });
    }
}
