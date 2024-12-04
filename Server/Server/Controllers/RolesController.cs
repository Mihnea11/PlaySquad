using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Repositories;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly RoleRepository _repository;

    public RolesController(RoleRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        return Ok(await _repository.GetAllAsync("Roles"));
    }

    [HttpPost]
    public async Task<IActionResult> AddRole([FromBody] Role role)
    {
        var result = await _repository.AddRoleAsync(role);
        return result > 0 ? Ok(new { Message = "Role added successfully" }) : BadRequest(new { Message = "Error adding role" });
    }
}
