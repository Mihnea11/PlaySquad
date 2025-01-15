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
            if (await _dbContext.SoccerFields.AnyAsync(sf => sf.Name == soccerField.Name))
            {
                throw new Exception("A soccer field with this name already exists.");
            }

            if (soccerField.OwnerId != 0)
            {
                var existingOwner = await _dbContext.Users.FindAsync(soccerField.OwnerId);
                if (existingOwner == null)
                {
                    throw new Exception("The specified owner does not exist.");
                }

                _dbContext.Entry(existingOwner).State = EntityState.Unchanged;
                soccerField.Owner = existingOwner;
            }
            _dbContext.SoccerFields.Add(soccerField);

            await _dbContext.SaveChangesAsync();

            return soccerField;
        }

        public async Task<SoccerField> UpdateSoccerFieldAsync(int id, SoccerField updatedSoccerField)
        {
            var soccerField = await _dbContext.SoccerFields.FirstOrDefaultAsync(sf => sf.Id == id);
            if (soccerField == null)
            {
                throw new Exception("Soccer field not found");
            }

            if (await _dbContext.SoccerFields.AnyAsync(sf => sf.Name == updatedSoccerField.Name && sf.Id != id))
            {
                throw new Exception("A soccer field with this name already exists.");
            }

            soccerField.Name = updatedSoccerField.Name;
            soccerField.Description = updatedSoccerField.Description;
            soccerField.Price = updatedSoccerField.Price;
            soccerField.MinCapacity = updatedSoccerField.MinCapacity;
            soccerField.MaxCapacity = updatedSoccerField.MaxCapacity;
            soccerField.Indoor = updatedSoccerField.Indoor;
            soccerField.Owner = updatedSoccerField.Owner;

            await _dbContext.SaveChangesAsync();
            return soccerField;
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
    }
}

