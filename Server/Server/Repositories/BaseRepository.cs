using System.Data;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Repositories
{
    public class BaseRepository<T>
    {
        protected readonly IDbConnection _connection;

        public BaseRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<T>> GetAllAsync(string tableName)
        {
            var query = $"SELECT * FROM \"{tableName}\"";
            return await _connection.QueryAsync<T>(query);
        }

        public async Task<T> GetByIdAsync(string tableName, int id)
        {
            var query = $"SELECT * FROM \"{tableName}\" WHERE id = @id";
            return await _connection.QueryFirstOrDefaultAsync<T>(query, new { id });
        }

        public async Task<int> AddAsync(string query, T entity)
        {
            return await _connection.ExecuteAsync(query, entity);
        }

        public async Task<int> UpdateAsync(string query, T entity)
        {
            return await _connection.ExecuteAsync(query, entity);
        }

        public async Task<int> DeleteAsync(string tableName, int id)
        {
            var query = $"DELETE FROM \"{tableName}\" WHERE id = @id";
            return await _connection.ExecuteAsync(query, new { id });
        }
    }
}
