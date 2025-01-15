using Microsoft.AspNetCore.Mvc;
using Server.Models.Entities;
using Server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var roles = await _roleService.GetAllRolesAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            try
            {
                var role = await _roleService.GetRoleByIdAsync(id);
                return Ok(role);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] Role role)
        {
            try
            {
                var createdRole = await _roleService.CreateRoleAsync(role);
                return CreatedAtAction(nameof(GetRoleById), new { id = createdRole.Id }, createdRole);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] Role updatedRole)
        {
            try
            {
                var role = await _roleService.UpdateRoleAsync(id, updatedRole);
                return Ok(role);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                var success = await _roleService.DeleteRoleAsync(id);
                return success ? NoContent() : NotFound(new { Message = "Role not found." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

    }
}