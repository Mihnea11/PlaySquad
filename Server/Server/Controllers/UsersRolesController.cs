using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Repositories;

[ApiController]
[Route("api/[controller]")]
public class UsersRolesController : ControllerBase
{
    private readonly UsersRolesRepository _repository;

    public UsersRolesController(UsersRolesRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsersRoles()
    {
        return Ok(await _repository.GetAllAsync("UsersRoles"));
    }

    [HttpPost]
    public async Task<IActionResult> AddUsersRoles([FromBody] UsersRoles usersRoles)
    {
        var result = await _repository.AddUsersRolesAsync(usersRoles);
        return result > 0 ? Ok(new { Message = "UsersRoles added successfully" }) : BadRequest(new { Message = "Error adding UsersRoles" });
    }
}
