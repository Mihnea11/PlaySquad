using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models.Entities;
using Server.Services.Interfaces;

namespace Server.Services.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly AppDbContext _dbContext;

        public BookingService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _dbContext.Bookings
                .Include(b => b.Field)
                .Include(b => b.Creator)
                .Include(b => b.WaitingList)
                .Include(b => b.ApprovedParticipants)
                .ToListAsync();
        }

        public async Task<Booking> GetBookingByIdAsync(int id)
        {
            var booking = await _dbContext.Bookings
                .Include(b => b.Field)
                .Include(b => b.Creator)
                .Include(b => b.WaitingList)
                .Include(b => b.ApprovedParticipants)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                throw new Exception("Booking not found");
            }
            return booking;
        }

        public async Task<Booking> CreateBookingAsync(Booking booking)
        {
            _dbContext.Bookings.Add(booking);
            await _dbContext.SaveChangesAsync();
            return booking;
        }

        public async Task<Booking> UpdateBookingAsync(int id, Booking updatedBooking)
        {
            var booking = await _dbContext.Bookings.FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                throw new Exception("Booking not found");
            }

            booking.Field = updatedBooking.Field;
            booking.Creator = updatedBooking.Creator;
            booking.WaitingList = updatedBooking.WaitingList;
            booking.ApprovedParticipants = updatedBooking.ApprovedParticipants;
            booking.MaxParticipants = updatedBooking.MaxParticipants;

            await _dbContext.SaveChangesAsync();
            return booking;
        }

        public async Task<bool> DeleteBookingAsync(int id)
        {
            var booking = await _dbContext.Bookings.FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                throw new Exception("Booking not found");
            }

            _dbContext.Bookings.Remove(booking);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddUserToWaitingListAsync(int bookingId, User user)
        {
            var booking = await _dbContext.Bookings.Include(b => b.WaitingList).FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null)
            {
                throw new Exception("Booking not found");
            }

            if (booking.WaitingList.Contains(user))
            {
                throw new Exception("User is already in the waiting list");
            }

            booking.WaitingList.Add(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveUserFromWaitingListAsync(int bookingId, int userId)
        {
            var booking = await _dbContext.Bookings.Include(b => b.WaitingList).FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null)
            {
                throw new Exception("Booking not found");
            }

            var user = booking.WaitingList.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                throw new Exception("User not found in the waiting list");
            }

            booking.WaitingList.Remove(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ApproveUserAsync(int bookingId, int userId)
        {
            var booking = await _dbContext.Bookings.Include(b => b.WaitingList).Include(b => b.ApprovedParticipants).FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null)
            {
                throw new Exception("Booking not found");
            }

            var user = booking.WaitingList.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                throw new Exception("User not found in the waiting list");
            }

            if (booking.ApprovedParticipants.Count >= booking.MaxParticipants)
            {
                throw new Exception("Max participants reached");
            }

            booking.WaitingList.Remove(user);
            booking.ApprovedParticipants.Add(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveUserFromApprovedListAsync(int bookingId, int userId)
        {
            var booking = await _dbContext.Bookings.Include(b => b.ApprovedParticipants).FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null)
            {
                throw new Exception("Booking not found");
            }

            var user = booking.ApprovedParticipants.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                throw new Exception("User not found in the waiting list");
            }

            booking.ApprovedParticipants.Remove(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
