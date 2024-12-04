using Server.Models;
using System.Data;

namespace Server.Repositories
{
    public class UsersRolesRepository : BaseRepository<UsersRoles>
    {
        public UsersRolesRepository(IDbConnection connection) : base(connection) { }

        public async Task<int> AddUsersRolesAsync(UsersRoles usersRoles)
        {
            var query = "INSERT INTO \"UsersRoles\" (user_id, role_id) VALUES (@UserId, @RoleId)";
            return await AddAsync(query, usersRoles);
        }
    }
}
