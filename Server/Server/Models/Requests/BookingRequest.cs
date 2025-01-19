using System.ComponentModel.DataAnnotations;

namespace Server.Models.Requests
{
    public class BookingRequest
    {
        [Required]
        public int FieldId { get; set; }

        [Required]
        public int CreatorId { get; set; }

        [Range(1, 50)]
        public int MaxParticipants { get; set; }

        public BookingRequest()
        {
            FieldId = 0;
            CreatorId = 0;
            MaxParticipants = 1;
        }
    }
}
