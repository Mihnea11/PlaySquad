using Server.Models;
using Server.Repositories.Interfaces;

namespace Server.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected List<TEntity> _entities = new List<TEntity>();

        public async Task<IEnumerable<TEntity>> GetAllAsync() =>
            await Task.FromResult(_entities);

        public async Task<TEntity?> GetByIdAsync(int id) =>
            await Task.FromResult(_entities.FirstOrDefault(e => (e as dynamic).Id == id));

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            _entities.Add(entity);
            return await Task.FromResult(entity);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var existing = _entities.FirstOrDefault(e => (e as dynamic).Id == (entity as dynamic).Id);
            if (existing != null)
            {
                _entities.Remove(existing);
                _entities.Add(entity);
            }
            return await Task.FromResult(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = _entities.FirstOrDefault(e => (e as dynamic).Id == id);
            if (entity != null)
            {
                _entities.Remove(entity);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }

    public class UserRepository : BaseRepository<User>, IUserRepository { }
    public class GameRepository : BaseRepository<Game>, IGameRepository { }
    public class ArenaRepository : BaseRepository<Arena>, IArenaRepository { }
    public class RoleRepository : BaseRepository<Role>, IRoleRepository { }
}
