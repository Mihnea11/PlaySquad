using Server.Models.Entities;

namespace Server.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role> GetDefaultRoleAsync();
        Task<Role> GetRoleByIdAsync(int id);
        Task<Role> CreateRoleAsync(Role role);
        Task<Role> UpdateRoleAsync(int id, Role updatedRole);
        Task<bool> DeleteRoleAsync(int id);
    }
}
