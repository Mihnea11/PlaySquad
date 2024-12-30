using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs;
using Server.Models;
using Server.Interfaces;

namespace Server.Services
{
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
}
