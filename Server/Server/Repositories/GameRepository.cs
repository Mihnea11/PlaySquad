using Server.Models;
using System.Data;

namespace Server.Repositories
{
    public class GameRepository : BaseRepository<Game>
    {
        public GameRepository(IDbConnection connection) : base(connection) { }

        public async Task<int> AddGameAsync(Game game)
        {
            var query = "INSERT INTO \"Games\" (type, start, end, arena_id, goals_team_a, goals_team_b) " +
                        "VALUES (@Type, @Start, @End, @ArenaId, @GoalsTeamA, @GoalsTeamB)";
            return await AddAsync(query, game);
        }
    }
}
