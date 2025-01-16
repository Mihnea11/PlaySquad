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
            await ValidateBookingAsync(booking);
            _dbContext.Bookings.Add(booking);
            await _dbContext.SaveChangesAsync();
            return booking;
        }

        public async Task<Booking> UpdateBookingAsync(int id, Booking updatedBooking)
        {
            var existingBooking = await _dbContext.Bookings.FirstOrDefaultAsync(b => b.Id == id);

            if (existingBooking == null)
            {
                throw new Exception("Booking not found.");
            }

            await ValidateBookingAsync(updatedBooking, isUpdate: true);

            existingBooking.Field = updatedBooking.Field;
            existingBooking.Creator = updatedBooking.Creator;
            existingBooking.WaitingList = updatedBooking.WaitingList;
            existingBooking.ApprovedParticipants = updatedBooking.ApprovedParticipants;
            existingBooking.MaxParticipants = updatedBooking.MaxParticipants;

            await _dbContext.SaveChangesAsync();

            return existingBooking;
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

        private async Task ValidateBookingAsync(Booking booking, bool isUpdate = false)
        {
            var creator = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == booking.CreatorId);
            if (creator == null)
            {
                throw new Exception("The specified creator does not exist.");
            }

            var field = await _dbContext.SoccerFields.FirstOrDefaultAsync(f => f.Id == booking.FieldId);
            if (field == null)
            {
                throw new Exception("The specified field does not exist.");
            }

            _dbContext.Entry(creator).State = EntityState.Unchanged;
            _dbContext.Entry(field).State = EntityState.Unchanged;

            booking.Creator = creator;
            booking.Field = field;

            if (booking.MaxParticipants <= 0)
            {
                throw new Exception("MaxParticipants must be greater than 0.");
            }
        }
    }
}
