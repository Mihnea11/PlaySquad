using Server.Models.Entities;

namespace Server.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(int id, User updatedUser);
        Task<bool> DeleteUserAsync(int id);

        Task<bool> AddStadiumAsOwnerAsync(int userId, SoccerField soccerField);
        Task<bool> RemoveStadiumFromOwnerAsync(int userId, int soccerFieldId);

        Task<bool> AddBookingToWaitingListAsync(int userId, int bookingId);
        Task<bool> RemoveBookingFromWaitingListAsync(int userId, int bookingId);

        Task<bool> AddBookingToApprovedListAsync(int userId, int bookingId);
        Task<bool> RemoveBookingFromApprovedListAsync(int userId, int bookingId);
    }
}
