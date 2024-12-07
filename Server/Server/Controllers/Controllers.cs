using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController<TEntity> : ControllerBase where TEntity : class
    {
        private readonly IBaseService<TEntity> _service;

        public BaseController(IBaseService<TEntity> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TEntity entity)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.AddAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = (created as dynamic).Id }, created);
        }

        // PATCH method to apply partial updates using JsonPatchDocument
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<TEntity> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest("Invalid patch document.");

            var entity = await _service.GetByIdAsync(id);
            if (entity == null)
                return NotFound();

            // Apply patch document to the entity
            patchDoc.ApplyTo(entity, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Save the updated entity back to the service
            await _service.UpdateAsync(entity);

            return NoContent(); // Successfully patched
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity == null)
                return NotFound();

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }

    [ApiController]
    [Route("api/users")]
    public class UsersController : BaseController<User>
    {
        public UsersController(IUserService userService) : base(userService) { }
    }

    [ApiController]
    [Route("api/games")]
    public class GamesController : BaseController<Game>
    {
        public GamesController(IGameService gameService) : base(gameService) { }
    }

    [ApiController]
    [Route("api/arenas")]
    public class ArenasController : BaseController<Arena>
    {
        public ArenasController(IArenaService arenaService) : base(arenaService) { }
    }

    [ApiController]
    [Route("api/roles")]
    public class RolesController : BaseController<Role>
    {
        public RolesController(IRoleService roleService) : base(roleService) { }
    }
}
