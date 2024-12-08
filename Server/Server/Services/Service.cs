using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs;
using Server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllAsync();
        Task<UserDTO?> GetByIdAsync(int id);
        Task<UserDTO> CreateAsync(UserCreateDTO createDto);
        Task<UserDTO> UpdateAsync(int id, UserCreateDTO updateDto);
        Task<bool> DeleteAsync(int id);
        Task AssignRoleToUserAsync(int userId, int roleId);
        Task RemoveRoleFromUserAsync(int userId, int roleId);
    }

public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDTO>> GetAllAsync()
        {
            var users = await _context.Users.Include(u => u.Roles).ToListAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task<UserDTO?> GetByIdAsync(int id)
        {
            var user = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == id);
            return user == null ? null : _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> CreateAsync(UserCreateDTO createDto)
        {
            var user = _mapper.Map<User>(createDto);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> UpdateAsync(int id, UserCreateDTO updateDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) throw new KeyNotFoundException("User not found.");
            _mapper.Map(updateDto, user);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task AssignRoleToUserAsync(int userId, int roleId)
        {
            var user = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == userId);
            var role = await _context.Roles.FindAsync(roleId);

            if (user == null || role == null)
                throw new KeyNotFoundException("User or Role not found.");

            user.Roles.Add(role);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveRoleFromUserAsync(int userId, int roleId)
        {
            var user = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == userId);
            var role = await _context.Roles.FindAsync(roleId);

            if (user == null || role == null)
                throw new KeyNotFoundException("User or Role not found.");

            user.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }
    }

    public interface IGameService
    {
        Task<IEnumerable<GameDTO>> GetAllAsync();
        Task<GameDTO?> GetByIdAsync(int id);
        Task<GameDTO> CreateAsync(GameCreateDTO createDto);
        Task<GameDTO> UpdateAsync(int id, GameCreateDTO updateDto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<GameDTO>> GetGamesByUserIdAsync(int userId);
    }

    public class GameService : IGameService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GameService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GameDTO>> GetAllAsync()
        {
            var games = await _context.Games.Include(g => g.Users).Include(g => g.Arena).ToListAsync();
            return _mapper.Map<IEnumerable<GameDTO>>(games);
        }

        public async Task<GameDTO?> GetByIdAsync(int id)
        {
            var game = await _context.Games.Include(g => g.Users).Include(g => g.Arena).FirstOrDefaultAsync(g => g.Id == id);
            return game == null ? null : _mapper.Map<GameDTO>(game);
        }

        public async Task<GameDTO> CreateAsync(GameCreateDTO createDto)
        {
            var game = _mapper.Map<Game>(createDto);
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            return _mapper.Map<GameDTO>(game);
        }

        public async Task<GameDTO> UpdateAsync(int id, GameCreateDTO updateDto)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null) throw new KeyNotFoundException("Game not found.");
            _mapper.Map(updateDto, game);
            await _context.SaveChangesAsync();
            return _mapper.Map<GameDTO>(game);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null) return false;
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<GameDTO>> GetGamesByUserIdAsync(int userId)
        {
            var user = await _context.Users.Include(u => u.Games).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new KeyNotFoundException("User not found.");

            return _mapper.Map<IEnumerable<GameDTO>>(user.Games);
        }
    }
    public interface IArenaService
    {
        Task<IEnumerable<ArenaDTO>> GetAllAsync();
        Task<ArenaDTO?> GetByIdAsync(int id);
        Task<ArenaDTO> CreateAsync(ArenaCreateDTO createDto);
        Task<ArenaDTO> UpdateAsync(int id, ArenaCreateDTO updateDto);
        Task<bool> DeleteAsync(int id);
    }

    public class ArenaService : IArenaService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ArenaService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ArenaDTO>> GetAllAsync()
        {
            var arenas = await _context.Arenas.ToListAsync();
            return _mapper.Map<IEnumerable<ArenaDTO>>(arenas);
        }

        public async Task<ArenaDTO?> GetByIdAsync(int id)
        {
            var arena = await _context.Arenas.FindAsync(id);
            return arena == null ? null : _mapper.Map<ArenaDTO>(arena);
        }

        public async Task<ArenaDTO> CreateAsync(ArenaCreateDTO createDto)
        {
            var arena = _mapper.Map<Arena>(createDto);
            _context.Arenas.Add(arena);
            await _context.SaveChangesAsync();
            return _mapper.Map<ArenaDTO>(arena);
        }

        public async Task<ArenaDTO> UpdateAsync(int id, ArenaCreateDTO updateDto)
        {
            var arena = await _context.Arenas.FindAsync(id);
            if (arena == null) throw new KeyNotFoundException("Arena not found.");
            _mapper.Map(updateDto, arena);
            await _context.SaveChangesAsync();
            return _mapper.Map<ArenaDTO>(arena);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var arena = await _context.Arenas.FindAsync(id);
            if (arena == null) return false;
            _context.Arenas.Remove(arena);
            await _context.SaveChangesAsync();
            return true;
        }
    }
    public interface IRoleService
    {
        Task<IEnumerable<RoleDTO>> GetAllAsync();
        Task<RoleDTO?> GetByIdAsync(int id);
        Task<RoleDTO> CreateAsync(RoleCreateDTO createDto);
        Task<RoleDTO> UpdateAsync(int id, RoleCreateDTO updateDto);
        Task<bool> DeleteAsync(int id);
    }

    public class RoleService : IRoleService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RoleService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleDTO>> GetAllAsync()
        {
            var roles = await _context.Roles.ToListAsync();
            return _mapper.Map<IEnumerable<RoleDTO>>(roles);
        }

        public async Task<RoleDTO?> GetByIdAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            return role == null ? null : _mapper.Map<RoleDTO>(role);
        }

        public async Task<RoleDTO> CreateAsync(RoleCreateDTO createDto)
        {
            var role = _mapper.Map<Role>(createDto);
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return _mapper.Map<RoleDTO>(role);
        }

        public async Task<RoleDTO> UpdateAsync(int id, RoleCreateDTO updateDto)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) throw new KeyNotFoundException("Role not found.");
            _mapper.Map(updateDto, role);
            await _context.SaveChangesAsync();
            return _mapper.Map<RoleDTO>(role);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return false;
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
