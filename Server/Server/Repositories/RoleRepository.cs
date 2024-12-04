using Server.Models;
using System.Data;

namespace Server.Repositories
{
    public class RoleRepository : BaseRepository<Role>
    {
        public RoleRepository(IDbConnection connection) : base(connection) { }

        public async Task<int> AddRoleAsync(Role role)
        {
            var query = "INSERT INTO \"Roles\" (name) VALUES (@Name)";
            return await AddAsync(query, role);
        }
    }
}
