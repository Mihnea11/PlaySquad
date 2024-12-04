using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Repositories;

[ApiController]
[Route("api/[controller]")]
public class GameUsersController : ControllerBase
{
    private readonly GameUserRepository _repository;

    public GameUsersController(GameUserRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetGameUsers()
    {
        return Ok(await _repository.GetAllAsync("GameUsers"));
    }

    [HttpPost]
    public async Task<IActionResult> AddGameUser([FromBody] GamesUsers gameUser)
    {
        var result = await _repository.AddGameUserAsync(gameUser);
        return result > 0 ? Ok(new { Message = "GameUser added successfully" }) : BadRequest(new { Message = "Error adding game user" });
    }
}
