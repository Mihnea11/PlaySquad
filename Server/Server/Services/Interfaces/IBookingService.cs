using Server.Models.Entities;

namespace Server.Services.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<Booking> GetBookingByIdAsync(int id);
        Task<Booking> CreateBookingAsync(Booking booking);
        Task<Booking> UpdateBookingAsync(int id, Booking updatedBooking);
        Task<bool> DeleteBookingAsync(int id);
        Task<bool> AddUserToWaitingListAsync(int bookingId, User user);
        Task<bool> RemoveUserFromWaitingListAsync(int bookingId, int userId);
        Task<bool> ApproveUserAsync(int bookingId, int userId);

        Task<bool> RemoveUserFromApprovedListAsync(int bookingId, int userId);
    }
}

