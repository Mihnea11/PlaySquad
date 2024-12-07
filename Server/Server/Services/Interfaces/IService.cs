using Server.Models;

namespace Server.Services.Interfaces
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(int id);
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(int id);
    }

    public interface IUserService : IBaseService<User> { }
    public interface IGameService : IBaseService<Game> { }
    public interface IArenaService : IBaseService<Arena> { }
    public interface IRoleService : IBaseService<Role> { }
}
