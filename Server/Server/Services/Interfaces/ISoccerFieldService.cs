using Server.Models.Entities;

namespace Server.Services.Interfaces
{
    public interface ISoccerFieldService
    {
        Task<IEnumerable<SoccerField>> GetAllSoccerFieldsAsync();
        Task<SoccerField> GetSoccerFieldByIdAsync(int id);
        Task<SoccerField> CreateSoccerFieldAsync(SoccerField soccerField);
        Task<SoccerField> UpdateSoccerFieldAsync(int id, SoccerField updatedSoccerField);
        Task<bool> DeleteSoccerFieldAsync(int id);
        Task<bool> AddBookingToSoccerFieldAsync(int soccerFieldId, Booking booking);
        Task<bool> RemoveBookingFromSoccerFieldAsync(int soccerFieldId, int bookingId);

        Task<ICollection<Booking>> GetBookingsBySoccerFieldIdAsync(int soccerFieldId);
    }
}
