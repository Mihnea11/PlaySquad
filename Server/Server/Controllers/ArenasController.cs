using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Repositories;

[ApiController]
[Route("api/[controller]")]
public class ArenasController : ControllerBase
{
    private readonly ArenaRepository _repository;

    public ArenasController(ArenaRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetArenas()
    {
        return Ok(await _repository.GetAllAsync("Arenas"));
    }

    [HttpPost]
    public async Task<IActionResult> AddArena([FromBody] Arena arena)
    {
        var result = await _repository.AddArenaAsync(arena);
        return result > 0 ? Ok(new { Message = "Arena added successfully" }) : BadRequest(new { Message = "Error adding arena" });
    }
}
