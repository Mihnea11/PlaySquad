using Server.Models;
using Server.Repositories.Interfaces;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        private readonly IBaseRepository<TEntity> _repository;

        public BaseService(IBaseRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync() =>
            await _repository.GetAllAsync();

        public async Task<TEntity?> GetByIdAsync(int id) =>
            await _repository.GetByIdAsync(id);

        public async Task<TEntity> AddAsync(TEntity entity) =>
            await _repository.AddAsync(entity);

        public async Task<TEntity> UpdateAsync(TEntity entity) =>
            await _repository.UpdateAsync(entity);

        public async Task<bool> DeleteAsync(int id) =>
            await _repository.DeleteAsync(id);
    }

    public class UserService : BaseService<User>, IUserService
    {
        public UserService(IUserRepository repository) : base(repository) { }
    }

    public class GameService : BaseService<Game>, IGameService
    {
        public GameService(IGameRepository repository) : base(repository) { }
    }

    public class ArenaService : BaseService<Arena>, IArenaService
    {
        public ArenaService(IArenaRepository repository) : base(repository) { }
    }

    public class RoleService : BaseService<Role>, IRoleService
    {
        public RoleService(IRoleRepository repository) : base(repository) { }
    }
}
