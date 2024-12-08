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
        Task<UserDTO> CreateAsync(UserCreateDTO createDto);
        Task<UserDTO?> GetByIdAsync(int id);
        Task<IEnumerable<UserDTO>> GetAllAsync();
        Task<UserDTO> UpdateAsync(int id, UserCreateDTO updateDto);
        Task<bool> DeleteAsync(int id);

        // Role management methods
        Task AssignRoleToUserAsync(int userId, int roleId);
        Task RemoveRoleFromUserAsync(int userId, int roleId);
        Task<IEnumerable<RoleDTO>> GetRolesForUserAsync(int userId);
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

        public async Task<UserDTO> CreateAsync(UserCreateDTO createDto)
        {
            var user = _mapper.Map<User>(createDto);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO?> GetByIdAsync(int id)
        {
            var user = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == id);
            return user == null ? null : _mapper.Map<UserDTO>(user);
        }

        public async Task<IEnumerable<UserDTO>> GetAllAsync()
        {
            var users = await _context.Users.Include(u => u.Roles).ToListAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
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

            if (user == null || role == null) throw new KeyNotFoundException("User or Role not found.");

            if (!user.Roles.Contains(role))
            {
                user.Roles.Add(role);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveRoleFromUserAsync(int userId, int roleId)
        {
            var user = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == userId);
            var role = await _context.Roles.FindAsync(roleId);

            if (user == null || role == null) throw new KeyNotFoundException("User or Role not found.");

            if (user.Roles.Contains(role))
            {
                user.Roles.Remove(role);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<RoleDTO>> GetRolesForUserAsync(int userId)
        {
            var user = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new KeyNotFoundException("User not found.");

            return _mapper.Map<IEnumerable<RoleDTO>>(user.Roles);
        }
    }


    public interface IGameService
    {
        Task<IEnumerable<GameDTO>> GetAllAsync();
        Task<GameDTO?> GetByIdAsync(int id);
        Task<GameDTO> CreateAsync(GameCreateDTO createDto);
        Task<GameDTO> UpdateAsync(int id, GameCreateDTO updateDto);
        Task<bool> DeleteAsync(int id);
        Task AddUserToGameAsync(int gameId, int userId);
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

        public async Task<GameDTO> CreateAsync(GameCreateDTO gameCreateDto)
        {
            var game = _mapper.Map<Game>(gameCreateDto);
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            return _mapper.Map<GameDTO>(game);
        }

        public async Task<IEnumerable<GameDTO>> GetAllAsync()
        {
            var games = await _context.Games.Include(g => g.Users).ToListAsync();
            return _mapper.Map<IEnumerable<GameDTO>>(games);
        }

        public async Task<GameDTO?> GetByIdAsync(int gameId)
        {
            var game = await _context.Games.Include(g => g.Users).FirstOrDefaultAsync(g => g.Id == gameId);
            if (game == null) throw new KeyNotFoundException("Game not found.");
            return _mapper.Map<GameDTO>(game);
        }

        public async Task<GameDTO> UpdateAsync(int gameId, GameCreateDTO gameUpdateDto)
        {
            var game = await _context.Games.FindAsync(gameId);
            if (game == null) throw new KeyNotFoundException("Game not found.");

            _mapper.Map(gameUpdateDto, game);
            await _context.SaveChangesAsync();

            return _mapper.Map<GameDTO>(game);
        }

        public async Task<bool> DeleteAsync(int gameId)
        {
            var game = await _context.Games.FindAsync(gameId);
            if (game == null) return false;

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task AddUserToGameAsync(int gameId, int userId)
        {
            var game = await _context.Games.Include(g => g.Users).FirstOrDefaultAsync(g => g.Id == gameId);
            var user = await _context.Users.FindAsync(userId);

            if (game == null || user == null) throw new KeyNotFoundException("Game or User not found.");

            if (!game.Users.Contains(user))
            {
                game.Users.Add(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveUserFromGameAsync(int gameId, int userId)
        {
            var game = await _context.Games.Include(g => g.Users).FirstOrDefaultAsync(g => g.Id == gameId);
            var user = await _context.Users.FindAsync(userId);

            if (game == null || user == null) throw new KeyNotFoundException("Game or User not found.");

            if (game.Users.Contains(user))
            {
                game.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<GameDTO>> GetGamesByUserIdAsync(int userId)
        {
            var games = await _context.Games.Include(g => g.Users).Where(g => g.Users.Any(u => u.Id == userId)).ToListAsync();
            return _mapper.Map<IEnumerable<GameDTO>>(games);
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

        public async Task<ArenaDTO> CreateAsync(ArenaCreateDTO arenaCreateDto)
        {
            var arena = _mapper.Map<Arena>(arenaCreateDto);
            _context.Arenas.Add(arena);
            await _context.SaveChangesAsync();
            return _mapper.Map<ArenaDTO>(arena);
        }

        public async Task<IEnumerable<ArenaDTO>> GetAllAsync()
        {
            var arenas = await _context.Arenas.ToListAsync();
            return _mapper.Map<IEnumerable<ArenaDTO>>(arenas);
        }

        public async Task<ArenaDTO?> GetByIdAsync(int arenaId)
        {
            var arena = await _context.Arenas.FindAsync(arenaId);
            if (arena == null) throw new KeyNotFoundException("Arena not found.");
            return _mapper.Map<ArenaDTO>(arena);
        }

        public async Task<ArenaDTO> UpdateAsync(int arenaId, ArenaCreateDTO arenaUpdateDto)
        {
            var arena = await _context.Arenas.FindAsync(arenaId);
            if (arena == null) throw new KeyNotFoundException("Arena not found.");

            _mapper.Map(arenaUpdateDto, arena);
            await _context.SaveChangesAsync();

            return _mapper.Map<ArenaDTO>(arena);
        }

        public async Task<bool> DeleteAsync(int arenaId)
        {
            var arena = await _context.Arenas.FindAsync(arenaId);
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

        public async Task<RoleDTO> CreateAsync(RoleCreateDTO roleCreateDto)
        {
            var role = _mapper.Map<Role>(roleCreateDto);
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return _mapper.Map<RoleDTO>(role);
        }

        public async Task<IEnumerable<RoleDTO>> GetAllAsync()
        {
            var roles = await _context.Roles.ToListAsync();
            return _mapper.Map<IEnumerable<RoleDTO>>(roles);
        }

        public async Task<RoleDTO?> GetByIdAsync(int roleId)
        {
            var role = await _context.Roles.FindAsync(roleId);
            if (role == null) throw new KeyNotFoundException("Role not found.");
            return _mapper.Map<RoleDTO>(role);
        }

        public async Task<RoleDTO> UpdateAsync(int roleId, RoleCreateDTO roleUpdateDto)
        {
            var role = await _context.Roles.FindAsync(roleId);
            if (role == null) throw new KeyNotFoundException("Role not found.");

            _mapper.Map(roleUpdateDto, role);
            await _context.SaveChangesAsync();

            return _mapper.Map<RoleDTO>(role);
        }

        public async Task<bool> DeleteAsync(int roleId)
        {
            var role = await _context.Roles.FindAsync(roleId);
            if (role == null) return false;

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return true;
        }
    }



}
