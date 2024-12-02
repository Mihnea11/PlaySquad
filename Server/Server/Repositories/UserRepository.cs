using Dapper;
using Server.Models;
namespace Server.Repositories
{
    public class UserRepository
    {
        private readonly Npgsql.NpgsqlConnection _connection;

        public UserRepository(Npgsql.NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var query = "SELECT * FROM \"Users\"";
            return await _connection.QueryAsync<User>(query);
        }

        public async Task<int> AddUserAsync(User user)
        {
            var query = "INSERT INTO \"Users\" (name) VALUES (@name)";
            return await _connection.ExecuteAsync(query, user);
        }
    }

}
