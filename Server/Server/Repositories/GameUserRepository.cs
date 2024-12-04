using Server.Models;
using System.Data;

namespace Server.Repositories
{
    public class GameUserRepository : BaseRepository<GamesUsers>
    {
        public GameUserRepository(IDbConnection connection) : base(connection) { }

        public async Task<int> AddGameUserAsync(GamesUsers gameUser)
        {
            var query = "INSERT INTO \"GameUsers\" (game_id, user_id) VALUES (@GameId, @UserId)";
            return await AddAsync(query, gameUser);
        }
    }
}
