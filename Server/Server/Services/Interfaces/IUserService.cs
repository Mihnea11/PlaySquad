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

        Task<ICollection<SoccerField>> GetOwnedFieldsAsync(int userId);
        Task<ICollection<Booking>> GetOwnedBookingsAsync(int userId);
        Task<ICollection<Booking>> GetRequestedBookingsAsync(int userId);
        Task<ICollection<Booking>> GetApprovedBookingsAsync(int userId);
    }
}
