using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models.Entities;
using Server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly AppDbContext _dbContext;

        public RoleService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Role> GetDefaultRoleAsync()
        {
            var defaultRole = await _dbContext.Roles.OrderBy(r => r.Id).FirstOrDefaultAsync();
            if (defaultRole == null)
            {
                throw new Exception("No roles available in the database.");
            }
            return defaultRole;
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _dbContext.Roles.ToListAsync();
        }

        public async Task<Role> GetRoleByIdAsync(int id)
        {
            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == id);
            if (role == null)
            {
                throw new Exception($"Role with ID {id} not found.");
            }
            return role;
        }


        public async Task<Role> CreateRoleAsync(Role role)
        {
            if (string.IsNullOrWhiteSpace(role.Name))
            {
                throw new Exception("Role name cannot be null or empty.");
            }

            if (await _dbContext.Roles.AnyAsync(r => r.Name == role.Name))
            {
                throw new Exception($"Role with name '{role.Name}' already exists.");
            }

            _dbContext.Roles.Add(role);
            await _dbContext.SaveChangesAsync();
            return role;
        }

        public async Task<Role> UpdateRoleAsync(int id, Role updatedRole)
        {
            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == id);
            if (role == null)
            {
                throw new Exception($"Role with ID {id} not found.");
            }

            if (string.IsNullOrWhiteSpace(updatedRole.Name))
            {
                throw new Exception("Updated role name cannot be null or empty.");
            }

            if (await _dbContext.Roles.AnyAsync(r => r.Name == updatedRole.Name && r.Id != id))
            {
                throw new Exception($"Role with name '{updatedRole.Name}' already exists.");
            }

            role.Name = updatedRole.Name;

            await _dbContext.SaveChangesAsync();
            return role;
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == id);
            if (role == null)
            {
                throw new Exception($"Role with ID {id} not found.");
            }

            if (await _dbContext.Users.AnyAsync(u => u.RoleId == id))
            {
                throw new Exception($"Cannot delete role with ID {id} as it is associated with existing users.");
            }

            _dbContext.Roles.Remove(role);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
