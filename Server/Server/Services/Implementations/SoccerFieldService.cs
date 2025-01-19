using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models.Entities;
using Server.Services.Interfaces;

namespace Server.Services.Implementations
{
    public class SoccerFieldService : ISoccerFieldService
    {
        private readonly AppDbContext _dbContext;

        public SoccerFieldService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<SoccerField>> GetAllSoccerFieldsAsync()
        {
            return await _dbContext.SoccerFields.Include(sf => sf.Owner).ToListAsync();
        }

        public async Task<SoccerField> GetSoccerFieldByIdAsync(int id)
        {
            var soccerField = await _dbContext.SoccerFields.Include(sf => sf.Owner).FirstOrDefaultAsync(sf => sf.Id == id);
            if (soccerField == null)
            {
                throw new Exception("Soccer field not found");
            }
            return soccerField;
        }

        public async Task<SoccerField> CreateSoccerFieldAsync(SoccerField soccerField)
        {
            await ValidateSoccerFieldAsync(soccerField);
            _dbContext.SoccerFields.Add(soccerField);
            await _dbContext.SaveChangesAsync();
            return soccerField;
        }

        public async Task<SoccerField> UpdateSoccerFieldAsync(int id, SoccerField soccerField)
        {
            var existingSoccerField = await _dbContext.SoccerFields
                .Include(sf => sf.Owner)
                .FirstOrDefaultAsync(sf => sf.Id == id);

            if (existingSoccerField == null)
            {
                throw new Exception("The soccer field does not exist.");
            }

            await ValidateSoccerFieldAsync(soccerField, isUpdate: true);

            existingSoccerField.Id = soccerField.Id;
            existingSoccerField.Name = soccerField.Name;
            existingSoccerField.Description = soccerField.Description;
            existingSoccerField.Price = soccerField.Price;
            existingSoccerField.MinCapacity = soccerField.MinCapacity;
            existingSoccerField.MaxCapacity = soccerField.MaxCapacity;
            existingSoccerField.Indoor = soccerField.Indoor;
            existingSoccerField.Owner = soccerField.Owner;

            await _dbContext.SaveChangesAsync();

            return existingSoccerField;
        }

        public async Task<bool> DeleteSoccerFieldAsync(int id)
        {
            var soccerField = await _dbContext.SoccerFields.FirstOrDefaultAsync(sf => sf.Id == id);
            if (soccerField == null)
            {
                throw new Exception("Soccer field not found");
            }

            _dbContext.SoccerFields.Remove(soccerField);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddBookingToSoccerFieldAsync(int soccerFieldId, Booking booking)
        {
            var soccerField = await _dbContext.SoccerFields.Include(sf => sf.Bookings).FirstOrDefaultAsync(sf => sf.Id == soccerFieldId);
            if (soccerField == null)
            {
                throw new Exception("Soccer field not found");
            }

            soccerField.Bookings.Add(booking);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<ICollection<Booking>> GetBookingsBySoccerFieldIdAsync(int soccerFieldId)
        {
            var bookings = await _dbContext.Bookings
                .Include(b => b.Field)               
                .Include(b => b.Creator)            
                .Include(b => b.WaitingList)        
                .Include(b => b.ApprovedParticipants) 
                .Where(b => b.Field.Id == soccerFieldId) 
                .ToListAsync();

            return bookings;
        }

        public async Task<bool> RemoveBookingFromSoccerFieldAsync(int soccerFieldId, int bookingId)
        {
            var soccerField = await _dbContext.SoccerFields.Include(sf => sf.Bookings).FirstOrDefaultAsync(sf => sf.Id == soccerFieldId);
            if (soccerField == null)
            {
                throw new Exception("Soccer field not found");
            }

            var booking = soccerField.Bookings.FirstOrDefault(b => b.Id == bookingId);
            if (booking == null)
            {
                throw new Exception("Booking not found for this soccer field");
            }

            soccerField.Bookings.Remove(booking);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        private async Task ValidateSoccerFieldAsync(SoccerField soccerField, bool isUpdate = false)
        {
            if (!isUpdate && await _dbContext.SoccerFields.AnyAsync(sf => sf.Name == soccerField.Name))
            {
                throw new Exception("A soccer field with this name already exists.");
            }

            var existingOwner = await _dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == soccerField.OwnerId);

            if (existingOwner == null)
            {
                throw new Exception("The specified owner does not exist.");
            }

            if (existingOwner.Role == null || existingOwner.Role.Name != "seller")
            {
                throw new Exception("The owner must have the role of 'Seller'.");
            }

            _dbContext.Entry(existingOwner).State = EntityState.Unchanged;
            soccerField.Owner = existingOwner;
        }
    }
}

