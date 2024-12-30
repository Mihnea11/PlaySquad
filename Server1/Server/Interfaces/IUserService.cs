using Server.DTOs;

namespace Server.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> CreateAsync(UserCreateDTO userCreateDTO);
        Task<UserDTO?> GetByIdAsync(int id);
        Task<IEnumerable<UserDTO>> GetAllAsync();
        Task<UserDTO> UpdatePartialAsync(int id, UserPatchDTO updateDto);
        Task<bool> DeleteAsync(int id);
        Task AssignRoleToUserAsync(int userId, int roleId);
        Task RemoveRoleFromUserAsync(int userId, int roleId);
        Task<IEnumerable<RoleDTO>> GetRolesForUserAsync(int userId);
    }
}
