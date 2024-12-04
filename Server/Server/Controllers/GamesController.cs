using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Repositories;

[ApiController]
[Route("api/[controller]")]
public class GamesController : ControllerBase
{
    private readonly GameRepository _repository;

    public GamesController(GameRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetGames()
    {
        return Ok(await _repository.GetAllAsync("Games"));
    }

    [HttpPost]
    public async Task<IActionResult> AddGame([FromBody] Game game)
    {
        var result = await _repository.AddGameAsync(game);
        return result > 0 ? Ok(new { Message = "Game added successfully" }) : BadRequest(new { Message = "Error adding game" });
    }
}
