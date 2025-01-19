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

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddStadiumAsOwnerAsync(int userId, SoccerField soccerField)
        {
            var user = await _dbContext.Users.Include(u => u.OwnedFields).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            soccerField.Owner = user;
            user.OwnedFields.Add(soccerField);
            _dbContext.SoccerFields.Add(soccerField);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveStadiumFromOwnerAsync(int userId, int soccerFieldId)
        {
            var user = await _dbContext.Users.Include(u => u.OwnedFields).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var soccerField = user.OwnedFields.FirstOrDefault(sf => sf.Id == soccerFieldId);
            if (soccerField == null)
            {
                throw new Exception("Soccer field not found");
            }

            user.OwnedFields.Remove(soccerField);
            _dbContext.SoccerFields.Remove(soccerField);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<ICollection<SoccerField>> GetOwnedFieldsAsync(int userId)
        {
            var user = await _dbContext.Users
                .Include(u => u.OwnedFields)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            return user.OwnedFields ?? new List<SoccerField>();
        }

        public async Task<ICollection<Booking>> GetOwnedBookingsAsync(int userId)
        {
            var user = await _dbContext.Users
                .Include(u => u.OwnedBookings)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            return user.OwnedBookings ?? new List<Booking>();
        }

        public async Task<ICollection<Booking>> GetRequestedBookingsAsync(int userId)
        {
            var user = await _dbContext.Users
                .Include(u => u.RequestedBookings)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            return user.RequestedBookings ?? new List<Booking>();
        }

        public async Task<ICollection<Booking>> GetApprovedBookingsAsync(int userId)
        {
            var user = await _dbContext.Users
                .Include(u => u.ApprovedBookings)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            return user.ApprovedBookings ?? new List<Booking>();
        }

    }
}
