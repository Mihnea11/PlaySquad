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
            Task<UserDTO> CreateAsync(UserCreateDTO userCreateDTO);
            Task<UserDTO?> GetByIdAsync(int id);
            Task<IEnumerable<UserDTO>> GetAllAsync();
            Task<UserDTO> UpdatePartialAsync(int id, UserPatchDTO updateDto);
            Task<bool> DeleteAsync(int id);
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

        public async Task<UserDTO> UpdatePartialAsync(int id, UserPatchDTO updateDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            user.Name = updateDto.Name ?? user.Name;
            user.Email = updateDto.Email ?? user.Email;
            user.Age = updateDto.Age.HasValue && updateDto.Age.Value != 0 ? updateDto.Age.Value : user.Age;
            user.City = updateDto.City ?? user.City;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Age = user.Age,
                City = user.City
            };
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
        Task<GameDTO> CreateAsync(GameCreateDTO gameCreateDTO);
        Task<GameDTO?> GetByIdAsync(int id);
        Task<IEnumerable<GameDTO>> GetAllAsync();
        Task<GameDTO> UpdatePartialAsync(int id, GamePatchDTO updateDto);
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

        public async Task<GameDTO> UpdatePartialAsync(int id, GamePatchDTO updateDto)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null) return null;

            game.Type = updateDto.Type ?? game.Type;
            game.StartTime = updateDto.StartTime.HasValue ? updateDto.StartTime.Value : game.StartTime;
            game.EndTime = updateDto.EndTime.HasValue ? updateDto.EndTime.Value : game.EndTime;
            game.ArenaId = updateDto.ArenaId.HasValue ? updateDto.ArenaId.Value : game.ArenaId;

            _context.Games.Update(game);
            await _context.SaveChangesAsync();

            return new GameDTO
            {
                Id = game.Id,
                Type = game.Type,
                StartTime = game.StartTime,
                EndTime = game.EndTime,
                ArenaId = game.ArenaId
            };
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
        Task<ArenaDTO> CreateAsync(ArenaCreateDTO arenaCreateDTO);
        Task<ArenaDTO?> GetByIdAsync(int id);
        Task<IEnumerable<ArenaDTO>> GetAllAsync();
        Task<ArenaDTO> UpdatePartialAsync(int id, ArenaPatchDTO updateDto);
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

        public async Task<ArenaDTO> UpdatePartialAsync(int id, ArenaPatchDTO updateDto)
        {
            var arena = await _context.Arenas.FindAsync(id);
            if (arena == null) return null;
            arena.Name = updateDto.Name ?? arena.Name;
            arena.Address = updateDto.Address ?? arena.Address;
            arena.MinPlayers = updateDto.MinPlayers.HasValue && updateDto.MinPlayers.Value != 0 ? updateDto.MinPlayers.Value : arena.MinPlayers;
            arena.MaxPlayers = updateDto.MaxPlayers.HasValue && updateDto.MaxPlayers.Value != 0 ? updateDto.MaxPlayers.Value : arena.MaxPlayers;
            arena.Type = updateDto.Type ?? arena.Type;
            arena.Price = updateDto.Price.HasValue && updateDto.Price.Value != 0 ? updateDto.Price.Value : arena.Price;

            _context.Arenas.Update(arena);
            await _context.SaveChangesAsync();

            return new ArenaDTO
            {
                Id = arena.Id,
                Name = arena.Name,
                Address = arena.Address,
                MinPlayers = arena.MinPlayers,
                MaxPlayers = arena.MaxPlayers,
                Type = arena.Type,
                Price = arena.Price
            };
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
        Task<RoleDTO> UpdatePartialAsync(int id, RolePatchDTO updateDto);
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

        public async Task<RoleDTO> UpdatePartialAsync(int id, RolePatchDTO updateDto)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return null;

            role.Name = updateDto.Name ?? role.Name;

            _context.Roles.Update(role);
            await _context.SaveChangesAsync();

            return new RoleDTO
            {
                Id = role.Id,
                Name = role.Name
            };
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
