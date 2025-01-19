namespace Server.Models.Responses
{
    public class BookingResponse
    {
        public int Id { get; set; }
        public int FieldId { get; set; }
        public string FieldName { get; set; }
        public UserResponse Creator { get; set; }
        public int MaxParticipants { get; set; }

        public BookingResponse()
        {
            FieldName = string.Empty;
            Creator = new UserResponse();
        }
    }
}