using Server.Models;

namespace Server.Repositories.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(int id);
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(int id);
    }

    public interface IUserRepository : IBaseRepository<User> { }
    public interface IRoleRepository : IBaseRepository<Role> { }
    public interface IArenaRepository : IBaseRepository<Arena> { }
    public interface IGameRepository : IBaseRepository<Game> { }
}