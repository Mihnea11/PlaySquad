using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.DTOs;
using Server.Services;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDTO userCreateDTO)
        {
            try
            {
                var userDto = await _userService.CreateAsync(userCreateDTO);
                return CreatedAtAction(nameof(GetUserById), new { id = userDto.Id }, userDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserCreateDTO updateDto)
        {
            try
            {
                var updatedUser = await _userService.UpdateAsync(id, updateDto);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var success = await _userService.DeleteAsync(id);
                if (!success)
                    return NotFound(new { Message = "User not found." });
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }


    [Route("api/games")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGames()
        {
            var games = await _gameService.GetAllAsync();
            return Ok(games);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGameById(int id)
        {
            var game = await _gameService.GetByIdAsync(id);
            if (game == null)
            {
                return NotFound(new { Message = "Game not found." });
            }
            return Ok(game);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGame([FromBody] GameCreateDTO gameCreateDTO)
        {
            try
            {
                var gameDto = await _gameService.CreateAsync(gameCreateDTO);
                return CreatedAtAction(nameof(GetGameById), new { id = gameDto.Id }, gameDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGame(int id, [FromBody] GameCreateDTO updateDto)
        {
            try
            {
                var updatedGame = await _gameService.UpdateAsync(id, updateDto);
                return Ok(updatedGame);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            try
            {
                var success = await _gameService.DeleteAsync(id);
                if (!success)
                    return NotFound(new { Message = "Game not found." });
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetGamesByUserId(int userId)
        {
            try
            {
                var games = await _gameService.GetGamesByUserIdAsync(userId);
                return Ok(games);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }

    [Route("api/arenas")]
    [ApiController]
    public class ArenaController : ControllerBase
    {
        private readonly IArenaService _arenaService;

        public ArenaController(IArenaService arenaService)
        {
            _arenaService = arenaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllArenas()
        {
            var arenas = await _arenaService.GetAllAsync();
            return Ok(arenas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArenaById(int id)
        {
            var arena = await _arenaService.GetByIdAsync(id);
            if (arena == null)
            {
                return NotFound(new { Message = "Arena not found." });
            }
            return Ok(arena);
        }

        [HttpPost]
        public async Task<IActionResult> CreateArena([FromBody] ArenaCreateDTO arenaCreateDTO)
        {
            try
            {
                var arenaDto = await _arenaService.CreateAsync(arenaCreateDTO);
                return CreatedAtAction(nameof(GetArenaById), new { id = arenaDto.Id }, arenaDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArena(int id, [FromBody] ArenaCreateDTO updateDto)
        {
            try
            {
                var updatedArena = await _arenaService.UpdateAsync(id, updateDto);
                return Ok(updatedArena);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArena(int id)
        {
            try
            {
                var success = await _arenaService.DeleteAsync(id);
                if (!success)
                    return NotFound(new { Message = "Arena not found." });
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
    [Route("api/games/{gameId}/users")]
    [ApiController]
    public class GameUserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GameUserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Add a user to a game
        [HttpPost("{userId}")]
        public async Task<IActionResult> AddUserToGame(int gameId, int userId)
        {
            var game = await _context.Games.FindAsync(gameId);
            var user = await _context.Users.FindAsync(userId);

            if (game == null)
            {
                return NotFound(new { Message = "Game not found." });
            }

            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            // Check if the user is already assigned to the game
            if (game.Users.Any(u => u.Id == userId))
            {
                return BadRequest(new { Message = "User is already assigned to the game." });
            }

            // Add the user to the game
            game.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User successfully added to the game." });
        }

        // Remove a user from a game
        [HttpDelete("{userId}")]
        public async Task<IActionResult> RemoveUserFromGame(int gameId, int userId)
        {
            var game = await _context.Games.FindAsync(gameId);
            var user = await _context.Users.FindAsync(userId);

            if (game == null)
            {
                return NotFound(new { Message = "Game not found." });
            }

            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            // Check if the user is assigned to the game
            if (!game.Users.Any(u => u.Id == userId))
            {
                return BadRequest(new { Message = "User is not assigned to this game." });
            }

            // Remove the user from the game
            game.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User successfully removed from the game." });
        }
    }
    [Route("api/roles")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // Get all roles
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(roles);
        }

        // Get a role by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await _roleService.GetByIdAsync(id);
            if (role == null)
            {
                return NotFound(new { Message = "Role not found." });
            }
            return Ok(role);
        }

        // Create a new role
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleCreateDTO roleCreateDTO)
        {
            try
            {
                var roleDto = await _roleService.CreateAsync(roleCreateDTO);
                return CreatedAtAction(nameof(GetRoleById), new { id = roleDto.Id }, roleDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // Update an existing role
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] RoleCreateDTO updateDto)
        {
            try
            {
                var updatedRole = await _roleService.UpdateAsync(id, updateDto);
                return Ok(updatedRole);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // Delete a role
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                var success = await _roleService.DeleteAsync(id);
                if (!success)
                    return NotFound(new { Message = "Role not found." });
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
    [Route("api/users/{userId}/roles")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserRoleController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("{roleId}")]
        public async Task<IActionResult> AssignRoleToUser(int userId, int roleId)
        {
            try
            {
                await _userService.AssignRoleToUserAsync(userId, roleId);
                return Ok(new { Message = "Role assigned successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{roleId}")]
        public async Task<IActionResult> RemoveRoleFromUser(int userId, int roleId)
        {
            try
            {
                await _userService.RemoveRoleFromUserAsync(userId, roleId);
                return Ok(new { Message = "Role removed successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }


}
