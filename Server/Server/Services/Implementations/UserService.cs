using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models.Entities;
using Server.Services.Interfaces;

namespace Server.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;

        public UserService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _dbContext.Users.Include(u => u.Role).ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _dbContext.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _dbContext.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            return user;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            if (await _dbContext.Users.AnyAsync(u => u.Email == user.Email))
            {
                throw new Exception("A user with this email already exists.");
            }

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUserAsync(int id, User updatedUser)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            if (await _dbContext.Users.AnyAsync(u => u.Email == updatedUser.Email && u.Id != id))
            {
                throw new Exception("A user with this email already exists.");
            }

            user.Email = updatedUser.Email;
            user.Name = updatedUser.Name;
            user.PictureUrl = updatedUser.PictureUrl;
            user.PasswordHash = updatedUser.PasswordHash;
            user.GoogleId = updatedUser.GoogleId;
            user.RoleId = updatedUser.RoleId;

            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}
