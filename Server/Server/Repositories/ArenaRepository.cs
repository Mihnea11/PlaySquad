using Server.Models;
using System.Data;

namespace Server.Repositories
{
    public class ArenaRepository : BaseRepository<Arena>
    {
        public ArenaRepository(IDbConnection connection) : base(connection) { }

        public async Task<int> AddArenaAsync(Arena arena)
        {
            var query = "INSERT INTO \"Arenas\" (name, city, address, owner_id, min_players, max_players, description, type, price) " +
                        "VALUES (@Name, @City, @Address, @OwnerId, @MinPlayers, @MaxPlayers, @Description, @Type, @Price)";
            return await AddAsync(query, arena);
        }
    }
}
