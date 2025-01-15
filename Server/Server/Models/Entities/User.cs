using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Server.Models.Entities
{
    public class User
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        [MaxLength(255)]
        public string GoogleId { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }
        public string PictureUrl { get; set; }

        public int RoleId { get; set; }

        [JsonIgnore]
        public Role? Role { get; set; }

        [JsonIgnore]
        public ICollection<SoccerField>? OwnedFields { get; set; } // Terenuri detinute

        [JsonIgnore]
        public ICollection<Booking>? OwnedBookings { get; set; } // Booking-uri unde este creator

        [JsonIgnore]
        public ICollection<Booking>? RequestedBookings { get; set; } // Request-uri la booking-uri

        [JsonIgnore]
        public ICollection<Booking>? ApprovedBookings { get; set; } // Booking-uri unde este participant aprobat

        public User()
        {
            Id = 0;
            Email = string.Empty;
            PasswordHash = string.Empty;
            GoogleId = string.Empty;
            Name = string.Empty;
            PictureUrl = string.Empty;
            RoleId = 0;
            OwnedFields = new List<SoccerField>();
            OwnedBookings = new List<Booking>();
            RequestedBookings = new List<Booking>();
            ApprovedBookings = new List<Booking>();
        }
    }
}
